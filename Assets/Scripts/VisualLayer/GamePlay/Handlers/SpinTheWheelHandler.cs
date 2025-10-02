using UnityEngine;
using VisualLayer.GamePlay.Popups.SpinTheWheelPopup;
using Zenject;

namespace VisualLayer.GamePlay.Handlers
{
    public class SpinTheWheelHandler : ISpinTheWheelHandler
    {
        [Inject]
        private SpinTheWheelPopup.Factory _spinTheWheelPopupFactory;
        public void Execute()
        {
            _spinTheWheelPopupFactory.Create();
        }
    }
}