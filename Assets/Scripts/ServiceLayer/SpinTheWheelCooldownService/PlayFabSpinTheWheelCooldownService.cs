using UnityEngine;

namespace ServiceLayer.SpinTheWheelCooldownService
{
    public class PlayFabSpinTheWheelCooldownService : BaseCooldownService, ISpinTheWheelCooldownService
    {
        protected override string CooldownKey => "LastDayClaimUtc";
        protected override int CooldownSeconds => 86400; //86400 for 1 Day
        protected override int RewardAmount => 0;
        protected override void OnClaim()
        {
            Debug.Log("here3");
        }
    }
}