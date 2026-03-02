using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DataLayer;
using DataLayer.Balances;
using ServiceLayer.PlayFabService;
using UnityEngine;
using Zenject;

namespace ServiceLayer.DataSyncService
{
    public interface ISyncLock
    {
        void LockSync();
        void UnlockSync();
    }
    
    public class DataSyncService : IInitializable, IDisposable, ISyncLock
    {
        private readonly IServerService _serverService;
        private readonly List<ISyncableService> _servicesToSync;
        
        private bool _isSyncScheduled;
        private bool _isLocked = true;

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
                serviceToSync.OnDataChanged += ScheduleSync;
        }

        public void Dispose()
        {
            foreach (var serviceToSync in _servicesToSync)
                serviceToSync.OnDataChanged -= ScheduleSync;
        }

        public void LockSync() 
        {
            _isLocked = true;
            Debug.Log("<color=yellow>[DataSync] Sync LOCKED - Upload blocked.</color>");
        }

        public void UnlockSync() 
        {
            _isLocked = false;
            Debug.Log("<color=cyan>[DataSync] Sync UNLOCKED - Ready to sync changes.</color>");
        }

        private void ScheduleSync()
        {
            if (_isLocked || _isSyncScheduled) return;

            _isSyncScheduled = true;
            SyncRoutine().Forget();
        }
        
        private async UniTaskVoid SyncRoutine()
        {
            await UniTask.Delay(TimeSpan.FromMilliseconds(800));

            if (_isLocked) 
            {
                _isSyncScheduled = false;
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
                    await SendDataInBatches(allData);
                }
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

            foreach (var kvp in allData)
            {
                currentBatch[kvp.Key] = kvp.Value;

                if (currentBatch.Count == batchSize)
                {
                    await SendBatchSafe(currentBatch);
                    currentBatch.Clear(); 
                }
            }

            if (currentBatch.Count > 0)
            {
                await SendBatchSafe(currentBatch);
            }
        }

        private async UniTask SendBatchSafe(Dictionary<string, string> batch)
        {
            try
            {
                await _serverService.SetUserData(batch);
                //Debug.Log($"<color=green>[DataSync] Successfully synced batch of {batch.Count} items</color>");
            }
            catch (Exception ex)
            {
                Debug.LogError($"[DataSync] Batch failed: {ex.Message}");
            }
        }
    }
}