using System.Collections;
using System.Collections.Generic;
using DataLayer.DataTypes;
using DataLayer.DataTypes.abilities;
using UnityEngine;
using VisualLayer.Components.UI;
using VisualLayer.Factories;
using VisualLayer.GamePlay.Abilities;
using VisualLayer.GamePlay.Handlers;
using VisualLayer.GamePlay.PlayerInput;
using VisualLayer.GamePlay.Popups.MusicMenuPopup;
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
            .Bind<InputDriven>()
            .FromComponentInHierarchy()
            .AsSingle();

        Container
            .BindFactory<int, Vector2, Item, ItemFactory>()
            .FromFactory<ItemFactoryImplementation>();

        Container
            .Bind<IMergeHandler>()
            .To<MergeHandler>()
            .AsSingle();
        
        Container
            .Bind<ISettingsMenuClickHandler>()
            .To<SettingsMenuClickHandler>()
            .AsSingle();
        
        Container.Bind<ISettingsMenuActions>()
            .To<SettingsMenuHandler>()
            .AsTransient();
        
        Container
            .Bind<IHudPlusCurrencyClickHandler>()
            .To<HudPlusCurrencyButtonClickHandler>()
            .AsSingle();
        
        Container
            .BindInterfacesAndSelfTo<GameLogicHandler>()
            .AsSingle();
        
        Container
            .Bind<GameLevelType>()
            .FromInstance(_levelType)
            .AsSingle();
    }
}
