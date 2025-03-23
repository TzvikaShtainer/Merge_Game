using Cysharp.Threading.Tasks;
using DataLayer;
using DataLayer.DataTypes;
using ServiceLayer.GameScenes;
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
        public async void Execute()
        {
            _loader.ResetData();
            await _loader.FadeIn();
            await UniTask.Delay(500);
            _loader.SetProgress(0.2f, "Loading Level 20%");

            await scenesService.UnloadLevelScene(GameLevelType.StartScreen);
                
            await UniTask.Delay(1000);
            _loader.SetProgress(0.5f, "Loading Level 50%");

            await scenesService.LoadLevelSceneIfNotLoaded(GameLevelType.GamePlay);
            await UniTask.Delay(500);
            _loader.SetProgress(1f, "Loading Level 100%");
            
            _loader.FadeOut();
        }
    }
}