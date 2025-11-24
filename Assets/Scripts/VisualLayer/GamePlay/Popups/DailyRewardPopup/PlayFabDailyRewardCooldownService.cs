using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DataLayer;
using ServiceLayer;
using ServiceLayer.PlayFabService;
using UnityEngine;
using VisualLayer.GamePlay.RewardSystem;
using Zenject;

namespace VisualLayer.GamePlay.Popups.DailyRewardPopup
{
    public class PlayFabDailyRewardCooldownService : BaseCooldownService,  IDailyRewardCooldownService
    {
        protected override string CooldownKey => "DailyRewardLastClaimUtc";
        protected override int CooldownSeconds => 1; //86400
        protected override int RewardAmount => 0; //no need
        
        private const string DayIndexKey = "DailyRewardDayIndex";
        private int _currentDayIndex;
        private const int MaxDaysInWeek = 7;
        
        [Inject] 
        private IServerService _serverService;
        
        [Inject] 
        private IDataLayer _dataLayer;
        
        [Inject] 
        private DailyRewardsConfig _dailyRewardsConfig;

        public override async UniTask LoadFromServer()
        {
            await base.LoadFromServer();

            var data = await  _serverService.GetUserData(DayIndexKey);
            
            if (data.TryGetValue(DayIndexKey, out var savedIndex) && int.TryParse(savedIndex, out var index))
                _currentDayIndex = index;
            else
                _currentDayIndex = 0;
            
            TryResetWeekIfNeeded();
            
        }
        protected override void OnClaim()
        {
            _currentDayIndex++;
            
            if (_currentDayIndex > MaxDaysInWeek)
                _currentDayIndex = MaxDaysInWeek;
            
            var reward = _dailyRewardsConfig.GetRewardForDay(_currentDayIndex);
            
            //Debug.Log($"🎁 Claimed Day {_currentDayIndex}: {reward.rewardName} (+{reward.coins})");
            
            //_dataLayer.Balances.AddCoins(reward.coins);
            
            _serverService.SetUserData(new Dictionary<string, string>
            {
                {CooldownKey, LastClaimTimeUtc.ToString("o")},
                {DayIndexKey,  _currentDayIndex.ToString()}
            }).Forget();
        }

private bool HasCooldownPassed()
{
    var elapsed = System.DateTime.UtcNow - LastClaimTimeUtc;
    return elapsed.TotalSeconds >= CooldownSeconds;
}

private void TryResetWeekIfNeeded()
{
    if (_currentDayIndex >= MaxDaysInWeek && HasCooldownPassed())
    {
        _currentDayIndex = 0; 

        _serverService.SetUserData(new Dictionary<string, string>
        {
            {CooldownKey, LastClaimTimeUtc.ToString("o")},
            {DayIndexKey,  _currentDayIndex.ToString()}
        }).Forget();
    }
}
        public int CurrentDayIndex()
        { 
            TryResetWeekIfNeeded();
            return _currentDayIndex;
        }

        public bool IsWeekComplete()  => _currentDayIndex >= MaxDaysInWeek;
    }
}