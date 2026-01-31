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
        public void Initialize()
        {
            _dataLayer.Balances.CoinsBalanceChanged += ScheduleSync;
            _dataLayer.Balances.HighScoreChanged += ScheduleSync;
            _dataLayer.Balances.ScoreChanged += ScheduleSync;
        }
        
        public void Dispose()
        {
            _dataLayer.Balances.CoinsBalanceChanged -= ScheduleSync;
            _dataLayer.Balances.HighScoreChanged -= ScheduleSync;
            _dataLayer.Balances.ScoreChanged -= ScheduleSync;
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
            await UniTask.Delay(TimeSpan.FromMilliseconds(500));
            _isSyncScheduled = false;

            try
            {
                var data = new Dictionary<string, string>
                {
                    { "Coins", _dataLayer.Balances.Coins.ToString() },
                    { "HighScore", _dataLayer.Balances.HighScore.ToString() },
                    { "CurrentScore", _dataLayer.Balances.CurrentScore.ToString() }
                };

                await _serverService.SetUserData(data);
            }
            catch (Exception ex)
            {
                Debug.LogError($"[DataSyncer] Sync Error: {ex.Message}");
            }
        }
    }
}