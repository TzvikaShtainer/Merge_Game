using Cysharp.Threading.Tasks;
using DataLayer;
using DataLayer.DataTypes;
using UnityEngine.SceneManagement;
using Zenject;

namespace ServiceLayer.GameScenes
{
    public class GameScenesService : IGameScenesService
    {

        #region Injects

        [Inject]
        private IDataLayer _dataLayer;
        
        [Inject]
        private ZenjectSceneLoader _zenjectSceneLoader;

        #endregion
        
        #region Methods
        public async UniTask LoadInfraSceneIfNotLoaded(InfraScreenType sceneType)
        {
            if (IsInfraSceneLoaded(sceneType))
            {
                return;
            }
            
            var metadata = _dataLayer.Metadata.GetInfraScreenMetadata(sceneType);
            await _zenjectSceneLoader.LoadSceneAsync(metadata.SceneBuildIndex, LoadSceneMode.Additive);
        }

        public async UniTask LoadLevelSceneIfNotLoaded(GameLevelType levelType)
        {
            if (IsLevelSceneLoaded(levelType))
            {
                return;
            }
            
            var metadata = _dataLayer.Metadata.GetLevelMetadata(levelType);
            await _zenjectSceneLoader.LoadSceneAsync(metadata.LevelSceneBuildIndex, LoadSceneMode.Additive);
        }

        public async UniTask UnloadInfraScreen(InfraScreenType sceneType)
        {
            var metadata = _dataLayer.Metadata.GetInfraScreenMetadata(sceneType);
            await SceneManager.UnloadSceneAsync(metadata.SceneBuildIndex);
        }

        public async UniTask UnloadLevelScene(GameLevelType levelType)
        {
            var metadata = _dataLayer.Metadata.GetLevelMetadata(levelType);
            await SceneManager.UnloadSceneAsync(metadata.LevelSceneBuildIndex);
        }
        
        private bool IsLevelSceneLoaded(GameLevelType levelType)
        {
            try
            {
                var metadata = _dataLayer.Metadata.GetLevelMetadata(levelType);
                var scene = SceneManager.GetSceneByBuildIndex(metadata.LevelSceneBuildIndex);
                return scene.isLoaded;
            }
            catch
            {
                // ignored
            }

            return false;
        }
        
        private bool IsInfraSceneLoaded(InfraScreenType sceneType)
        {
            try
            {
                var metadata = _dataLayer.Metadata.GetInfraScreenMetadata(sceneType);
                var scene = SceneManager.GetSceneByBuildIndex(metadata.SceneBuildIndex);
                return scene.isLoaded;
            }
            catch
            {
                // ignored
            }

            return false;
        }
        
        #endregion
    }
}