using ServiceLayer.Signals.SignalsClasses;
using VisualLayer.GamePlay.Handlers;
using Zenject;

namespace ServiceLayer.SaveSystem
{
    public class GamePlayReadyNotifier: IInitializable
    {
        [Inject]
        private SignalBus _signalBus;
        
        [Inject]
        private ISaveSystem _saveSystem;
        
        [Inject]
        private GameLogicHandler _gameLogicHandler;
        
        public void Initialize()
        {
             _saveSystem.Init(_gameLogicHandler);
            _signalBus.Fire(new GamePlayReadySignal());
        }
    }
}