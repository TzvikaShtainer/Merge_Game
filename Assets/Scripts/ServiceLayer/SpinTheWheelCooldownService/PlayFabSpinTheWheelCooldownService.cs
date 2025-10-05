namespace ServiceLayer.SpinTheWheelCooldownService
{
    public class PlayFabSpinTheWheelCooldownService : BaseCooldownService, ISpinTheWheelCooldownService
    {
        protected override string CooldownKey => "LastDayClaimUtc";
        protected override int CooldownSeconds => 600;
        protected override int RewardAmount => 0;
        protected override void OnClaim()
        {
           
        }
    }
}