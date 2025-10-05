using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DataLayer;
using PlayFab;
using PlayFab.ClientModels;
using ServiceLayer.PlayFabService;
using UnityEngine;
using Zenject;

namespace ServiceLayer.HourlyCoinsService
{
    public class PlayFabHourlyCoinsService : IHourlyCoinsService
    {
        [Inject]
        private IServerService _serverService;
        
        [Inject]
        private IDataLayer _dataLayer;
        
        private const int CooldownSeconds = 3600;  //3600 for 1 hour
        private DateTime _lastClaimUtc;
        private int _hourlyCoinsToAdd = 10;

        private TimeSpan _serverOffset;
        private DateTime _serverTimeAtLoad;
        
        public DateTime LastClaimTimeUtc  => _lastClaimUtc;
        
        public async UniTask LoadFromServer()
        {
            var serverTime = await GetServerTimeUtc();
            _serverTimeAtLoad = serverTime;
            
            _serverOffset = serverTime -  DateTime.UtcNow;
            
            var data = await _serverService.GetUserData("LastClaimUtc");
            if (data.TryGetValue("LastClaimUtc", out var lastClaimUtcSaved))
            {
                _lastClaimUtc = DateTime.Parse(lastClaimUtcSaved).ToUniversalTime();
                //Debug.Log("have lasttime");
            }
            else
            {
                _lastClaimUtc = DateTime.MinValue;
                //Debug.Log("else");
            }
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
            
            _serverService.SetUserData(new Dictionary<string, string>
            {
                {"LastClaimUtc", _lastClaimUtc.ToString("o")},
            });
            
            _dataLayer.Balances.AddCoins(_hourlyCoinsToAdd);
        }
        
        public int GetHourlyCoinsAmount() => _hourlyCoinsToAdd;
    }
}