using System;
using EasyUI.PickerWheelUI;

namespace VisualLayer.GamePlay.Handlers.StartScene
{
    public interface IWheelHandler
    {
        public event Action OnSpinStarted;
        public event Action<WheelPiece> OnSpinEnded;
        
        public void SpinWheel();

        void SetWheel(PickerWheel pickerWheel);

        public bool CanSpin(out TimeSpan remaining);
    }
}