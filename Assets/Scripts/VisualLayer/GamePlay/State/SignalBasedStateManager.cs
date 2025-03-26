using ServiceLayer.Signals.SignalsClasses;
using UnityEngine;
using VisualLayer.GamePlay.Handlers;
using Zenject;

namespace VisualLayer.GamePlay.State
{
    public class SignalBasedStateManager : IInitializable
    {
        #region Injects

        [Inject]
        private SignalBus _signalBus;
        
        [Inject]
        private GameEndHandler _gameEndHandler;

        #endregion
        
        public void Initialize()
        {
            _signalBus.Subscribe<ReachedColliderLose>(OnPlayerLose);
        }

        private void OnPlayerLose()
        {
            _gameEndHandler.Execute();
        }
    }
}