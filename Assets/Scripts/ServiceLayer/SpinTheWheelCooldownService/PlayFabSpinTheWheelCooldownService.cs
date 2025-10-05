namespace ServiceLayer.SpinTheWheelCooldownService
{
    public class PlayFabSpinTheWheelCooldownService : BaseCooldownService, ISpinTheWheelCooldownService
    {
        protected override string CooldownKey => "LastDayClaimUtc";
        protected override int CooldownSeconds => 2; //86400 for 1 Day
        protected override int RewardAmount => 0;
        protected override void OnClaim()
        {
           
        }
    }
}