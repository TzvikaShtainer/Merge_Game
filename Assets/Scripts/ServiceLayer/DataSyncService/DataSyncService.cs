using System;
using System.Collections.Generic;
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
        [Inject] 
        private IServerService _serverService;
        
        [Inject] 
        private  IDataLayer _dataLayer;
        
        private bool _isSyncScheduled;
        private List<ISyncableService> _servicesToSync;
        
        public DataSyncService([InjectOptional] List<ISyncableService> servicesToSync)
        {
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

            var combinedData = new Dictionary<string, string>();

            foreach (var serviceToGetDataFrom in _servicesToSync)
            {
                var data = serviceToGetDataFrom.GetSyncData();
                
                foreach (var kvp in data)
                {
                    combinedData[kvp.Key] = kvp.Value;
                }
            }
            
            try
            {
                await _serverService.SetUserData(combinedData);
            }
            catch (Exception ex)
            {
                Debug.LogError($"[DataSyncer] Sync Error: {ex.Message}");
            }
        }
    }
}