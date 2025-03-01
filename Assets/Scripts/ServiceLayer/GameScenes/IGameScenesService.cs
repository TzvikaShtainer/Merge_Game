using Cysharp.Threading.Tasks;
using DataLayer.DataTypes;

namespace ServiceLayer.GameScenes
{
    public interface IGameScenesService
    {
        UniTask LoadInfraSceneIfNotLoaded(InfraScreenType sceneType);
        UniTask LoadLevelSceneIfNotLoaded(GameLevelType levelType);
        UniTask UnloadInfraScreen(InfraScreenType sceneType);
        UniTask UnloadLevelScene(GameLevelType levelType);
    }
}