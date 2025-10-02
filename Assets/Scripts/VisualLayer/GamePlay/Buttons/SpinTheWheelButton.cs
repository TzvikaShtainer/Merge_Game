using VisualLayer.GamePlay.Handlers;
using Zenject;

namespace VisualLayer.GamePlay.Buttons
{
    public class SpinTheWheelButton : UIButtonFeedback
    {
        [Inject]
        private ISpinTheWheelHandler  _spinTheWheelHandler;
        public void OnClick()
        {
            base.OnClick();
            _spinTheWheelHandler.Execute();
        }
    }
}