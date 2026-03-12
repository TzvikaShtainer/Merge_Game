using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using ServiceLayer.Signals.SignalsClasses;
using UniRx;
using UnityEngine;
using Zenject;

namespace ServiceLayer.NotificationsService
{
    public class NotificationFlowManager : IInitializable, IDisposable
    {
        public ReactiveProperty<bool> IsNotificationsFlowCompleted = new ReactiveProperty<bool>();
        
        [Inject]
        private List<IInitializableNotification>  _notificationsList;
        
        [Inject]
        private SignalBus _signalBus;
        
        public void Initialize()
        {
            _signalBus.Subscribe<GameReadyForNotificationsSignal>(OnGameReady);
        }

        private void OnGameReady()
        {
            RunNotificationSequence().Forget();
        }

        public async UniTask RunNotificationSequence()
        {
            IsNotificationsFlowCompleted.Value = false;
            
            var sortedList = _notificationsList.OrderBy(n => n.Priority).ToList();
            foreach (var notification in sortedList)
            {
                // while (_internetService.IsHandlingConnectionLoss) 
                // {
                //     await UniTask.Yield(); 
                // }
                
                if (await notification.ShouldShow())
                {
                    await notification.Show();
                }
            }
            
            IsNotificationsFlowCompleted.Value = true;
        }
        
        public void Dispose()
        {
            _signalBus.Unsubscribe<GameReadyForNotificationsSignal>(OnGameReady);
        }
    }
}