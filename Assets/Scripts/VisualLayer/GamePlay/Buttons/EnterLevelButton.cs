using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VisualLayer.GamePlay.Handlers;
using Zenject;

namespace VisualLayer.GamePlay.Buttons
{
    public class EnterLevelButton : UIButtonFeedback
    {
        [Inject]
        private IStartGameClickHandler _enterLevelHandler;

        private bool _isBlocked = true;
        private int _timeToBlockBtnInSec = 7;

        private void Start()
        {
            StartCooldown().Forget();
        }

        private async UniTask StartCooldown()
        {
            _isBlocked = true;
            
            await UniTask.Delay(TimeSpan.FromSeconds(_timeToBlockBtnInSec), DelayType.Realtime);
            _isBlocked = false;
        }


        public void OnClick()
        {
            if (_isBlocked)
                return;
            
            base.OnClick();
            _enterLevelHandler.Execute();
        }
    }
}