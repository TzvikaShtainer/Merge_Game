using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DataLayer;
using PlayFab;
using PlayFab.ClientModels;
using ServiceLayer.DataSyncService;
using ServiceLayer.PlayFabService;
using ServiceLayer.TImeProvider;
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
        
        [Inject]
        private ITimeProviderService _timeProviderService;
        
        protected abstract string CooldownKey { get; }
        protected abstract int CooldownSeconds { get; }
        protected abstract int RewardAmount { get; }

        private DateTime _initialServerTime;
        private double _startupTimestamp;
        private DateTime _lastClaimUtc;
        private bool _isInitialized;

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
            _initialServerTime = await _timeProviderService.GetServerTimeUtc();
            
            _startupTimestamp = Time.realtimeSinceStartupAsDouble;
            
            var data = await _serverService.GetUserData(CooldownKey);
            if (data.TryGetValue(CooldownKey, out var saved) && DateTime.TryParse(saved, out var parsedTime))
            {
                _lastClaimUtc = parsedTime.ToUniversalTime();
            }
            else
            {
                _lastClaimUtc = DateTime.MinValue;
            }

            _isInitialized = true;
            NotifyDataChanged();
        }
       
        private DateTime GetCurrentServerTime()
        {
            if (!_isInitialized) return DateTime.UtcNow;

            // חישוב כמה זמן עבר באמת מאז הסנכרון האחרון
            double elapsedSinceSync = Time.realtimeSinceStartupAsDouble - _startupTimestamp;
            return _initialServerTime.AddSeconds(elapsedSinceSync);
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

        public virtual void Claim()
        {
            if (!CanClaim(out _))
            {
                Debug.LogWarning($"[{CooldownKey}] Claim attempted before cooldown finished.");
                return;
            }

            _lastClaimUtc = GetCurrentServerTime();
            
            OnClaim();
            NotifyDataChanged();
        }

        protected abstract void OnClaim();

        public int GetRewardAmount() => RewardAmount;

        protected void NotifyDataChanged() => OnDataChanged?.Invoke();
    }
}