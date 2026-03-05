using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using ServiceLayer.PlayFabService;
using UnityEngine;
using Zenject;

namespace ServiceLayer.DataSyncService
{
    public class DataSyncService : IInitializable, IDisposable, ISyncLock
    {
        private readonly IServerService _serverService;
        private readonly List<ISyncableService> _servicesToSync;
        
        private bool _isSyncScheduled;
        private bool _isLocked = true;
        private bool _hasPendingChanges;

        public DataSyncService(
            IServerService serverService, 
            [Inject(Source = InjectSources.Any)] List<ISyncableService> servicesToSync)
        {
            _serverService = serverService;
            _servicesToSync = servicesToSync;
        }

        public void Initialize()
        {
            foreach (var serviceToSync in _servicesToSync)
            {
                serviceToSync.OnDataChanged += ScheduleSync;
                //Debug.Log($"<color=white>[DataSync] Subscribed to {serviceToSync.GetType().Name}</color>");
            }
            
            Application.quitting += OnApplicationQuitting;
        }

        public void Dispose()
        {
            foreach (var serviceToSync in _servicesToSync)
                serviceToSync.OnDataChanged -= ScheduleSync;
        }

        public void LockSync() 
        {
            _isLocked = true;
            //Debug.Log("<color=orange>[DataSync] 🔒 Sync LOCKED - Changes will be queued but not sent.</color>");
        }

        public void UnlockSync() 
        {
            _isLocked = false;
            //Debug.Log("<color=cyan>[DataSync] 🔓 Sync UNLOCKED - Processing pending changes: " + _hasPendingChanges + "</color>");
            
            if (_hasPendingChanges)
            {
                ScheduleSync();
            }
        }

        private void OnApplicationQuitting()
        {
            //Debug.Log("<color=red>[DataSync] ⚠️ Application Quitting! Attempting emergency sync...</color>");
            ForceSyncImmediate().Forget();
        }
        
        private async UniTask ForceSyncImmediate()
        {
            var allData = new Dictionary<string, string>();
            foreach (var service in _servicesToSync)
            {
                var data = service.GetSyncData();
                if (data != null)
                {
                    foreach (var kvp in data) allData[kvp.Key] = kvp.Value;
                }
            }

            if (allData.Count > 0)
            {
                await _serverService.SetUserData(allData);
                //Debug.Log("<color=green>[DataSync] ✅ Emergency sync completed successfully.</color>");
            }
        }
        private void ScheduleSync()
        {
            // Debug.Log("<color=white>[DataSync] Change detected. IsLocked: " + _isLocked + ", IsScheduled: " + _isSyncScheduled + "</color>");

            if (_isLocked) 
            {
                _hasPendingChanges = true;
                //Debug.Log("<color=yellow>[DataSync] Pending change recorded (System is Locked).</color>");
                return;
            }

            if (_isSyncScheduled) 
            {
                return;
            }

            _isSyncScheduled = true;
            _hasPendingChanges = false;
            //Debug.Log("<color=magenta>[DataSync] Scheduling sync routine (Debounce 800ms starting...)</color>");
            SyncRoutine().Forget();
        }
        
        private async UniTaskVoid SyncRoutine()
        {
            await UniTask.Delay(TimeSpan.FromMilliseconds(800));

            if (_isLocked) 
            {
                //Debug.LogWarning("[DataSync] SyncRoutine aborted: System was re-locked during delay.");
                _isSyncScheduled = false;
                _hasPendingChanges = true;
                return;
            }

            try
            {
                var allData = new Dictionary<string, string>();

                foreach (var service in _servicesToSync)
                {
                    var data = service.GetSyncData();
                    if (data == null) continue;
                    
                    foreach (var kvp in data) 
                    {
                        if (!string.IsNullOrEmpty(kvp.Key))
                            allData[kvp.Key] = kvp.Value;
                    }
                }

                if (allData.Count > 0)
                {
                    //Debug.Log($"<color=blue>[DataSync] Starting upload of {allData.Count} unique data keys...</color>");
                    await SendDataInBatches(allData);
                }
                else
                {
                    //Debug.Log("<color=grey>[DataSync] SyncRoutine finished: No data found to sync.</color>");
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"[DataSync] CRITICAL ERROR in SyncRoutine: {ex.Message}");
            }
            finally
            {
                _isSyncScheduled = false;
            }
        }

        private async UniTask SendDataInBatches(Dictionary<string, string> allData)
        {
            const int batchSize = 10; 
            var currentBatch = new Dictionary<string, string>(batchSize);
            int totalSent = 0;

            foreach (var kvp in allData)
            {
                currentBatch[kvp.Key] = kvp.Value;

                if (currentBatch.Count == batchSize)
                {
                    await SendBatchSafe(currentBatch);
                    totalSent += currentBatch.Count;
                    currentBatch.Clear(); 
                }
            }

            if (currentBatch.Count > 0)
            {
                await SendBatchSafe(currentBatch);
                totalSent += currentBatch.Count;
            }
            
            //Debug.Log($"<color=green>[DataSync] Finished syncing total of {totalSent} items to server.</color>");
        }

        private async UniTask SendBatchSafe(Dictionary<string, string> batch)
        {
            try
            {
                await _serverService.SetUserData(batch);
                //Debug.Log($"<color=#00ff00>[DataSync] Successfully synced batch of {batch.Count} items.</color>");
            }
            catch (Exception ex)
            {
                //Debug.LogError($"[DataSync] Batch send failed: {ex.Message}");
                _hasPendingChanges = true; 
            }
        }
    }
}