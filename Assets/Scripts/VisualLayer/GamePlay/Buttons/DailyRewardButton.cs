using VisualLayer.GamePlay.Buttons;
using Zenject;

namespace VisualLayer.GamePlay.Handlers.StartScene
{
    public class DailyRewardButton : UIButtonFeedback
    {
        [Inject]
        private IDailyRewardHandler _dailyRewardHandler;
        public void OnClick()
        {
            base.OnClick();
            _dailyRewardHandler.Execute();
        }
    }
}