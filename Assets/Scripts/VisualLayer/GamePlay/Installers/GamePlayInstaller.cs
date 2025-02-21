using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VisualLayer.GamePlay.PlayerInput;
using VisualLayer.MergeItems;
using VisualLayer.MergeItems.SpawnLogic;
using Zenject;

public class GamePlayInstaller : MonoInstaller<GamePlayInstaller>
{
    public override void InstallBindings()
    {
        Container
            .Bind<IPlayerInput>()
            .To<DesktopInputManager>()
            .AsSingle()
            .IfNotBound();
    }
}
