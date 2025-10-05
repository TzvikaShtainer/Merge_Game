using System;
using EasyUI.PickerWheelUI;
using ServiceLayer.SpinTheWheelCooldownService;
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
        
        [Inject] 
        private ISpinTheWheelCooldownService _cooldownService;
        
        private PickerWheel _pickerWheel;

        public void SetWheel(PickerWheel wheel)
        { 
            _pickerWheel = wheel;
            _pickerWheel.OnSpinEnd(OnWheelSpinEnd);
        }

        private void OnWheelSpinEnd(WheelPiece wheelPiece)
        {
            _rewardSystem.GrandReward(wheelPiece);
            _cooldownService.Claim();
            OnSpinEnded?.Invoke(wheelPiece);
        }
        
        public bool CanSpin(out TimeSpan remaining)
        {
            return _cooldownService.CanClaim(out remaining);
        }

        
        public void SpinWheel()
        {
            if (!_cooldownService.CanClaim(out _)) return;
            
            OnSpinStarted?.Invoke();
            
            _pickerWheel?.Spin();
        }
    }
}