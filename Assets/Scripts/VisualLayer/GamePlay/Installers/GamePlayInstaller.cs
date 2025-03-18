using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VisualLayer.Factories;
using VisualLayer.GamePlay.Handlers;
using VisualLayer.GamePlay.PlayerInput;
using VisualLayer.GamePlay.UI;
using VisualLayer.MergeItems;
using VisualLayer.MergeItems.MergeSystem;
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

        Container
            .BindFactory<int, Item, ItemFactory>()
            .FromFactory<ItemFactoryImplementation>();

        Container
            .Bind<IMergeHandler>()
            .To<MergeHandler>()
            .AsSingle();
        
        Container
            .Bind<IHudBackClickHandler>()
            .To<HudBackClickHandler>()
            .AsSingle();
        
        Container
            .BindInterfacesAndSelfTo<GameLogicHandler>()
            .AsSingle();
            
    }
}
