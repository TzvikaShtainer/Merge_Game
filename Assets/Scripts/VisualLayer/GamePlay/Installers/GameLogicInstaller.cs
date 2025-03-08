using UnityEngine;
using VisualLayer.GamePlay.Handlers;
using Zenject;

namespace VisualLayer.GamePlay.Installers
{
    public class GameLogicInstaller : MonoInstaller<GameLogicInstaller>
    {
        public override void InstallBindings()
        {
            Container
                .Bind<IInitializable>()
                .To<GameLogicHandler>()
                .AsSingle();
        }
    }
}