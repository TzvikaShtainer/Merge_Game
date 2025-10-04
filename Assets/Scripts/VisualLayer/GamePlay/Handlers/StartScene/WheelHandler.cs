using System;
using EasyUI.PickerWheelUI;
using UnityEngine;
using VisualLayer.GamePlay.Popups.SpinTheWheelPopup;
using VisualLayer.GamePlay.RewardSystem;
using Zenject;

namespace VisualLayer.GamePlay.Handlers.StartScene
{
    public class WheelHandler : IWheelHandler
    {
        public event Action OnSpinStarted;
        public event Action<WheelPiece> OnSpinEnded;
        
        [Inject]
        private IRewardSystem  _rewardSystem;
        
        private PickerWheel _pickerWheel;

        public void SetWheel(PickerWheel wheel)
        { 
            _pickerWheel = wheel;
            _pickerWheel.OnSpinEnd(OnWheelSpinEnd);
        }

        private void OnWheelSpinEnd(WheelPiece wheelPiece)
        {
            _rewardSystem.GrandReward(wheelPiece);
            
            OnSpinEnded?.Invoke(wheelPiece);
        }
        
        public void SpinWheel()
        {
            _pickerWheel?.Spin();
            
            OnSpinStarted?.Invoke();
        }
    }
}