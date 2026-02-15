using Cysharp.Threading.Tasks;
using VisualLayer.GamePlay.Handlers.StartScene;
using VisualLayer.GamePlay.Popups.DailyRewardPopup;
using VisualLayer.GamePlay.Popups.SpinTheWheelPopup;
using Zenject;

namespace ServiceLayer.NotificationsService
{
    public class SpinTheWheelNotification : IInitializableNotification
    {
        [Inject] 
        private IWheelHandler  _wheelHandler;
        
        [Inject] 
        private SpinTheWheelPopup.Factory _popupFactory;

        public int Priority => 101;

        public UniTask<bool> ShouldShow()
        {
            bool canClaim = _wheelHandler.CanSpin(out var remaining);
            return UniTask.FromResult(canClaim);
        }

        public async UniTask Show()
        {
            var popup = _popupFactory.Create();
            await popup.WaitForClose();
        }
    }
}