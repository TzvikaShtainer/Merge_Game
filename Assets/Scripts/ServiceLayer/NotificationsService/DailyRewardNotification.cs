using Cysharp.Threading.Tasks;
using UnityEngine;
using VisualLayer.GamePlay.Popups.DailyRewardPopup;
using Zenject;

namespace ServiceLayer.NotificationsService
{
    public class DailyRewardNotification : IInitializableNotification
    {
        [Inject] 
        private PlayFabDailyRewardCooldownService _dailyRewardService;
        
        [Inject] 
        private DailyRewardPopup.Factory _popupFactory;
        
        public int Priority => 100;
        public UniTask<bool> ShouldShow()
        {
            bool canClaim = _dailyRewardService.CanClaim(out var remaining);
            return UniTask.FromResult(canClaim);
        }

        public async UniTask Show()
        {
            var popup = _popupFactory.Create();
            await popup.WaitForClose();
        }
    }
}