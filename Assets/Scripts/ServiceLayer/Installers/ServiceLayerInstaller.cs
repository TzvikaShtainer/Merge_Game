using ServiceLayer.GameScenes;
using UnityEngine;
using Zenject;

namespace ServiceLayer.Installers
{
    [CreateAssetMenu(menuName = "Merge/Data/Service Layer Installer", fileName = "Service Layer Installer")]
    public class ServiceLayerInstaller : ScriptableObjectInstaller<ServiceLayerInstaller>
    {
        public override void InstallBindings()
        {
            Container
                .Bind<IGameScenesService>()
                .To<GameScenesService>()
                .AsSingle();
        }
    }
}