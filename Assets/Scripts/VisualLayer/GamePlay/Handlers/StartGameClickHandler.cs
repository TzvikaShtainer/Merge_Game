using Cysharp.Threading.Tasks;
using DataLayer;
using DataLayer.DataTypes;
using ServiceLayer.GameScenes;
using ServiceLayer.PlayFabService;
using VisualLayer.Loader;
using Zenject;

namespace VisualLayer.GamePlay.Handlers
{
    public class StartGameClickHandler : IStartGameClickHandler
    {
        [Inject] 
        private ILoader _loader;
        
        [Inject]
        private IGameScenesService scenesService;

        [Inject] 
        private IDataLayer _dataLayer;
        
        [Inject]
        private IServerService _serverService;
        
        
        public async void Execute()
        {
            _loader.ResetData();
            await _loader.FadeIn();
            //await UniTask.Delay(500);
            //_loader.SetProgress(0.2f, "Loading Level 20%");
            await _loader.AnimateProgressTo(0.2f, 0.5f);

            await _serverService.Login();
            await _dataLayer.Balances.LoadFromServer();
            
            await UniTask.Delay(1000);
            
            await scenesService.UnloadLevelScene(GameLevelType.StartScreen);
                
            //await UniTask.Delay(1000);
            //_loader.SetProgress(0.5f, "Loading Level 50%");
            await _loader.AnimateProgressTo(0.5f, 1f);


            await scenesService.LoadLevelSceneIfNotLoaded(GameLevelType.GamePlay);
            //await UniTask.Delay(500);
            //_loader.SetProgress(1f, "Loading Level 100%");
            await _loader.AnimateProgressTo(1f, 0.5f);

            
            _loader.FadeOut();
        }
    }
}