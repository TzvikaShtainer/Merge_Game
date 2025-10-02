using VisualLayer.GamePlay.Handlers;
using Zenject;

namespace VisualLayer.GamePlay.Buttons
{
    public class SpinTheWheelStartScreenButton : UIButtonFeedback
    {
        [Inject]
        private ISpinTheWheelStartScreenHandler  _spinTheWheelStartScreenHandler;
        public void OnClick()
        {
            base.OnClick();
            _spinTheWheelStartScreenHandler.Execute();
        }
    }
}