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
    public class PlayFabHourlyCoinsService : BaseCooldownService, IHourlyCoinsService
    {
        [Inject] private IDataLayer _dataLayer;

        protected override string CooldownKey => "LastHourlyClaimUtc";
        protected override int CooldownSeconds => 3600; //3600
        protected override int RewardAmount => 10;
        protected override void OnClaim()
        {
            Debug.Log("here2");
            _dataLayer.Balances.AddCoins(RewardAmount);
        }
    }
}