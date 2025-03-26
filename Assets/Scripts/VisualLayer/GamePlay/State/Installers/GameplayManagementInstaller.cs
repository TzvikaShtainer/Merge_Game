using UnityEngine;
using VisualLayer.GamePlay.Handlers;
using Zenject;

namespace VisualLayer.GamePlay.State.Installers
{
    [CreateAssetMenu(menuName = "Merge/State/Signal Based State Management Installer", fileName = "State Management Installer")]
    public class GameplayManagementInstaller : ScriptableObjectInstaller<GameplayManagementInstaller>
    {
        public override void InstallBindings()
        {
            Container
                .BindInterfacesTo<SignalBasedStateManager>()
                .AsSingle();

            Container
                .Bind<GameEndHandler>()
                .AsTransient();
        }
    }
}