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
    public class DataSyncService : IInitializable, IDisposable
    {
        private IServerService _serverService;
        private bool _isSyncScheduled;
        private List<ISyncableService> _servicesToSync;
        
        public DataSyncService(
            IServerService serverService, 
            [Inject(Source = InjectSources.Any)] List<ISyncableService> servicesToSync)
        {
            _serverService = serverService;
            _servicesToSync = servicesToSync ?? new List<ISyncableService>();
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
        
        private void ScheduleSync()
        {
            if (_isSyncScheduled)
                return;

            _isSyncScheduled = true;
            SyncRoutine().Forget();
        }
        
        private async UniTaskVoid SyncRoutine()
        {
            await UniTask.Delay(TimeSpan.FromMilliseconds(800));
            _isSyncScheduled = false;

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

            var dataList = allData.ToList();
            int batchSize = 10; 

            for (int i = 0; i < dataList.Count; i += batchSize)
            {
                var batchDict = dataList
                    .Skip(i)
                    .Take(batchSize)
                    .ToDictionary(x => x.Key, x => x.Value);

                try
                {
                    Debug.Log($"[DataSync] Sending batch {i / batchSize + 1}: {string.Join(", ", batchDict.Keys)}");
            
                    await _serverService.SetUserData(batchDict);
            
                    Debug.Log($"<color=green>[DataSync] Batch {i / batchSize + 1} Successful!</color>");
                }
                catch (Exception ex)
                {
                    Debug.LogError($"[DataSync] Batch starting at index {i} failed: {ex.Message}");
                    Debug.LogError($"[DataSync] Failed keys in this batch: {string.Join(", ", batchDict.Keys)}");
                }
            }
        }
    }
}