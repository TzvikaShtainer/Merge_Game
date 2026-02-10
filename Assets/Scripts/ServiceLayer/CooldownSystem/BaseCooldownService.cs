using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DataLayer;
using PlayFab;
using PlayFab.ClientModels;
using ServiceLayer.DataSyncService;
using ServiceLayer.PlayFabService;
using UnityEngine;
using Zenject;

namespace ServiceLayer
{
    public abstract class BaseCooldownService: ICooldownService, ISyncableService
    {
        public event Action OnDataChanged;
        
        [Inject]
        private IServerService _serverService;
        
        [Inject]
        private IDataLayer _dataLayer;
        
        protected abstract string CooldownKey { get; }
        protected abstract int CooldownSeconds { get; }
        protected abstract int RewardAmount { get; }

        private DateTime _lastClaimUtc;
        private TimeSpan _serverOffset;

        public DateTime LastClaimTimeUtc => _lastClaimUtc;
        
        public virtual Dictionary<string, string> GetSyncData()
        {
            return new Dictionary<string, string>
            {
                {CooldownKey, _lastClaimUtc.ToString("o")}
            };
        }
        public virtual async UniTask LoadFromServer()
        {
            var serverTime = await GetServerTimeUtc();
            _serverOffset = serverTime - DateTime.UtcNow;

            var data = await _serverService.GetUserData(CooldownKey);
            if (data.TryGetValue(CooldownKey, out var saved))
                _lastClaimUtc = DateTime.Parse(saved).ToUniversalTime();
            else
                _lastClaimUtc = DateTime.MinValue;
        }
        
        public async UniTask<DateTime> GetServerTimeUtc()
        {
            var tcs = new UniTaskCompletionSource<DateTime>();
             
            PlayFabClientAPI.GetTime(new GetTimeRequest(),
                results =>
                {
                    tcs.TrySetResult(results.Time.ToUniversalTime());
                },
                error =>
                {
                    Debug.LogError($"Failed to get server time: {error.ErrorMessage}");
                    tcs.TrySetResult(DateTime.UtcNow);
                });

            return await tcs.Task;
        }

        private DateTime GetCurrentServerTime()
        {
            return DateTime.UtcNow + _serverOffset;
        }

        public bool CanClaim(out TimeSpan timeRemaining)
        {
            var now = GetCurrentServerTime();
            var nextAvailable = _lastClaimUtc.AddSeconds(CooldownSeconds);

            if (now >= nextAvailable)
            {
                timeRemaining = TimeSpan.Zero;
                return true;
            }

            timeRemaining = nextAvailable - now;
            return false;
        }

        public void Claim()
        {
            var now = GetCurrentServerTime();
            _lastClaimUtc = now;
            
            OnClaim();
            
            OnDataChanged?.Invoke();
        }
        protected abstract void OnClaim();
        public int GetRewardAmount() => RewardAmount;
        
        protected void NotifyDataChanged() => OnDataChanged?.Invoke();
    }
}