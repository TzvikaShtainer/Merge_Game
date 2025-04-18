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
    
    [Header("Abilities Settings")]
    [Header("Destroy Lowest Ability")]
    [SerializeField]
    private AbilityDataSO destroyLowestAbilityData;
    
    [Header("Shake Box Ability")]
    [SerializeField]
    private AbilityDataSO shakeBoxAbilityData;
    
    [SerializeField]
    private GameObject shakeBoxAbilityJarPrefab;

    [SerializeField] 
    private Camera mainGameplayCamera;
    
    [Header("Upgrade Specific FruitAbility")]
    [SerializeField]
    private AbilityDataSO upgradeSpecificFruitAbilityData ;
    
    [Header("Destroy Specific Fruit Ability")]
    [SerializeField]
    private AbilityDataSO destroySpecificFruitAbilityData;
    
    
    
    
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
        
        
        //-----------------Abilities Binds:-------------
        Container
            .Bind<AbilityManager>()
            .AsSingle();
        
        //DestroyAllLowestLevelFruitsAbility ability bind:
        Container
            .Bind<IAbility>()
            .To<DestroyAllLowestLevelFruitsAbility>()
            .AsSingle();
        
        Container
            .Bind<AbilityDataSO>()
            .FromInstance(destroyLowestAbilityData)
            .WhenInjectedInto<DestroyAllLowestLevelFruitsAbility>();
        
        //ShakeBoxAbility ability bind:
        Container
            .Bind<IAbility>()
            .To<ShakeBoxAbility>()
            .AsSingle();
        
        Container
            .Bind<AbilityDataSO>()
            .FromInstance(shakeBoxAbilityData)
            .WhenInjectedInto<ShakeBoxAbility>();
        
        Container
            .Bind<Transform>()
            .WithId("ShakeBoxJar")
            .FromInstance(shakeBoxAbilityJarPrefab.transform)
            .AsCached();
        
        Container
            .Bind<Camera>()
            .WithId("MainGameplayCamera")
            .FromInstance(mainGameplayCamera)
            .AsSingle();
        
        //UpgradeSpecificFruitAbility ability bind:
        Container
            .Bind<IAbility>()
            .To<UpgradeSpecificFruitAbility>()
            .AsSingle();
        
        Container
            .Bind<AbilityDataSO>()
            .FromInstance(upgradeSpecificFruitAbilityData)
            .WhenInjectedInto<UpgradeSpecificFruitAbility>();
        
        //DestroySpecificFruitAbility ability bind:
        Container
            .Bind<IAbility>()
            .To<DestroySpecificFruitAbility>()
            .AsSingle();
        
        Container
            .Bind<AbilityDataSO>()
            .FromInstance(destroySpecificFruitAbilityData)
            .WhenInjectedInto<DestroySpecificFruitAbility>();
    }
}
