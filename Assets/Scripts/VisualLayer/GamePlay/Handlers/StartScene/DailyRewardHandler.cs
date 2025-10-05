using VisualLayer.GamePlay.Popups.DailyRewardPopup;
using VisualLayer.GamePlay.Popups.SpinTheWheelPopup;
using Zenject;

namespace VisualLayer.GamePlay.Handlers.StartScene
{
    public class DailyRewardHandler : IDailyRewardHandler
    {
        [Inject]
        private DailyRewardPopup.Factory _dailyRewardPopupFactory;
        
        public void Execute()
        {
            _dailyRewardPopupFactory.Create();
        }
    }
}