using Cysharp.Threading.Tasks;
using DataLayer.DataTypes;

namespace ServiceLayer.GameScenes
{
    public class GameScenesService : IGameScenesService
    {
        public UniTask LoadInfraSceneIfNotLoaded(InfraScreenType sceneType)
        {
            throw new System.NotImplementedException();
        }

        public UniTask LoadLevelSceneIfNotLoaded(GameLevelType levelType)
        {
            throw new System.NotImplementedException();
        }

        public UniTask UnloadInfraScreen(InfraScreenType sceneType)
        {
            throw new System.NotImplementedException();
        }

        public UniTask UnloadLevelScene(GameLevelType levelType)
        {
            throw new System.NotImplementedException();
        }
    }
}