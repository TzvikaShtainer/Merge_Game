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
    
    public class DataSyncService : IInitializable, IDisposable
    {
        private readonly IServerService _serverService;
        private readonly List<ISyncableService> _servicesToSync;
        
        private bool _isSyncScheduled;
        private bool _isLocked = false; // חסימה לוגית בזמן טעינה

        public DataSyncService(
            IServerService serverService, 
            [Inject(Source = InjectSources.Any)] List<ISyncableService> servicesToSync)
        {
            _serverService = serverService;
            _servicesToSync = servicesToSync ?? new List<ISyncableService>();
        }

        public void Initialize()
        {
            // נרשמים לאירועים מיד, אבל השליטה היא דרך ה-Lock
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
            // אם המערכת נעולה (בטעינה) - אנחנו פשוט מתעלמים מהבקשה לסנכרן
            if (_isLocked) return;
            
            if (_isSyncScheduled) return;

            _isSyncScheduled = true;
            SyncRoutine().Forget();
        }
        
        private async UniTaskVoid SyncRoutine()
        {
            // מחכים מעט כדי לאסוף שינויים נוספים (Debounce)
            await UniTask.Delay(TimeSpan.FromMilliseconds(800));
            _isSyncScheduled = false;

            // בדיקה נוספת למקרה שהסנכרון ננעל בזמן ההמתנה
            if (_isLocked) return;

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

            if (allData.Count == 0) return;

            await SendDataInBatches(allData);
        }

        private async UniTask SendDataInBatches(Dictionary<string, string> allData)
        {
            var dataList = allData.ToList();
            const int batchSize = 10; 

            for (int i = 0; i < dataList.Count; i += batchSize)
            {
                var batchDict = dataList
                    .Skip(i)
                    .Take(batchSize)
                    .ToDictionary(x => x.Key, x => x.Value);

                try
                {
                    await _serverService.SetUserData(batchDict);
                    //Debug.Log($"<color=green>[DataSync] Successfully synced batch {i / batchSize + 1}</color>");
                }
                catch (Exception ex)
                {
                    Debug.LogError($"[DataSync] Batch failed: {ex.Message}");
                }
            }
        }
    }
}