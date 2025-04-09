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
using VisualLayer.GamePlay.UI;
using VisualLayer.MergeItems;
using VisualLayer.MergeItems.MergeSystem;
using VisualLayer.MergeItems.SpawnLogic;
using Zenject;

public class GamePlayInstaller : MonoInstaller<GamePlayInstaller>
{
    
    [SerializeField] 
    private GameLevelType _levelType;
    
    [SerializeField]
    private AbilityDataSO destroyLowestAbilityData;
    
    [SerializeField]
    private AbilityDataSO ability2;
    
    [SerializeField]
    private AbilityDataSO ability3;
    
    [SerializeField]
    private AbilityDataSO ability4;
    
    
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
        
        
        //Abilities Binds:
        Container
            .Bind<AbilityManager>()
            .AsSingle();
        
        Container
            .Bind<IAbility>()
            .To<DestroyAllLowestLevelFruitsAbility>()
            .AsSingle();
        
        Container
            .Bind<AbilityDataSO>()
            .FromInstance(destroyLowestAbilityData)
            .WhenInjectedInto<DestroyAllLowestLevelFruitsAbility>();

    }
}
