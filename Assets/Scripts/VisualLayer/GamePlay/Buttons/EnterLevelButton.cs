using System;
using Cysharp.Threading.Tasks;
using ServiceLayer.NotificationsService;
using UniRx;
using UnityEngine;
using VisualLayer.GamePlay.Handlers;
using Zenject;

namespace VisualLayer.GamePlay.Buttons
{
    public class EnterLevelButton : UIButtonFeedback
    {
        [Inject]
        private IStartGameClickHandler _enterLevelHandler;

        [Inject] 
        private NotificationFlowManager _notificationFlowManager;

        private bool _isBlocked = true;

        private void Start()
        {
            InitializeReactiveFlow();
        }

        private void InitializeReactiveFlow()
        {
            _notificationFlowManager.IsNotificationsFlowCompleted
                .Subscribe(completed =>
                {
                    _isBlocked = !completed;
                })
                .AddTo(this);
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