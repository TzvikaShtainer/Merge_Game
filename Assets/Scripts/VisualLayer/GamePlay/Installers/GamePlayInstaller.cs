using System.Collections;
using System.Collections.Generic;
using DataLayer.DataTypes;
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
    
    [SerializeField] 
    private GameLevelType _levelType;
    
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
        
        Container
            .Bind<GameLevelType>()
            .FromInstance(_levelType)
            .AsSingle();
        
        Container
            .Bind<IStartGameClickHandler>()
            .To<StartGameClickHandler>()
            .AsSingle();
            
    }
}
