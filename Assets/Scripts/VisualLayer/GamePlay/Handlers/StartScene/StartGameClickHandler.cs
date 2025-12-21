using Cysharp.Threading.Tasks;
using DataLayer;
using DataLayer.DataTypes;
using ServiceLayer.GameScenes;
using ServiceLayer.PlayFabService;
using ServiceLayer.Signals.SignalsClasses;
using ServiceLayer.Utilis;
using VisualLayer.Loader;
using Zenject;

namespace VisualLayer.GamePlay.Handlers
{
    public class StartGameClickHandler : IStartGameClickHandler
    {
        [Inject] 
        private ILoader _loader;
        
        [Inject]
        private IGameScenesService _scenesService;

        [Inject] 
        private IDataLayer _dataLayer;
        
        [Inject]
        private IServerService _serverService;
        
        [Inject]
        private GameStartupCoordinator  _gameStartupCoordinator;
        
        [Inject] 
        private SignalBus _signalBus;
        
        
        public async void Execute() 
        {
            await _loader.InitLoader();
            
            await _loader.AnimateProgressTo(0.2f, 0.5f);
            
            await UniTask.Delay(1000);
            
            await _scenesService.UnloadLevelScene(GameLevelType.StartScreen);
            
            await _loader.AnimateProgressTo(0.5f, 1f);
            
            await _scenesService.LoadLevelSceneIfNotLoaded(GameLevelType.GamePlay);
            
            await _gameStartupCoordinator.LoadAllDataFromServer();
            await _gameStartupCoordinator.LoadAllDataFromDevice();
            
            await _loader.AnimateProgressTo(1f, 0.5f);
            
            await _loader.FadeOut();
            
            _signalBus.Fire<UnpauseInputSignal>();
        }
    }
}