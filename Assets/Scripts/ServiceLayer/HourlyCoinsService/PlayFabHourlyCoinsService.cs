using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DataLayer;
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
        
        private const int CooldownSeconds = 60;  //3600
        private DateTime _lastClaimUtc;
        private int _hourlyCoinsToAdd = 10;
        
        public DateTime LastClaimTimeUtc  => _lastClaimUtc;
        
        public async UniTask LoadFromServer()
        {
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

        public bool CanClaim(out TimeSpan timeRemaining)
        {
            var now = DateTime.UtcNow;
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
            _lastClaimUtc = DateTime.UtcNow;
            _serverService.SetUserData(new Dictionary<string, string>
            {
                {"LastClaimUtc", _lastClaimUtc.ToString("o")},
            });
            
            _dataLayer.Balances.AddCoins(_hourlyCoinsToAdd);
        }
        
        public int GetHourlyCoinsAmount()
        {
            return _hourlyCoinsToAdd;
        }
    }
}