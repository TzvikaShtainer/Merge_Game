using System.Collections.Generic;
using DataLayer.DataTypes.abilities;
using UnityEngine;
using VisualLayer.GamePlay.Abilities;
using Zenject;

namespace VisualLayer.GamePlay.Installers
{
    public class AbilitiesInstaller: MonoInstaller<AbilitiesInstaller>
    {
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
        
        [Header("Destroy Items After Continue Ability")]
        [SerializeField]
        private AbilityDataSO destroyItemsAfterContinueAbilityData;
        
        [Inject] private AbilityManager _abilityManager;
        [Inject] private List<IAbility> _allAbilities;
        
        public override void InstallBindings()
        {
            // Container
            //     .Bind<AbilityManager>()
            //     .AsSingle();
                    
            //DestroyAllLowestLevelFruitsAbility ability bind:
            Container
                .Bind<IAbility>()
                .To<DestroyAllLowestLevelFruitsAbility>()
                .AsCached();
                    
            Container
                .Bind<AbilityDataSO>()
                .FromInstance(destroyLowestAbilityData)
                .WhenInjectedInto<DestroyAllLowestLevelFruitsAbility>();
                    
            //ShakeBoxAbility ability bind:
            Container
                .Bind<IAbility>()
                .To<ShakeBoxAbility>()
                .AsCached();
                    
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
                .AsCached();
                    
            Container
                .Bind<AbilityDataSO>()
                .FromInstance(upgradeSpecificFruitAbilityData)
                .WhenInjectedInto<UpgradeSpecificFruitAbility>();
                    
            //DestroySpecificFruitAbility ability bind:
            Container
                .Bind<IAbility>()
                .To<DestroySpecificFruitAbility>()
                .AsCached();
                    
            Container
                .Bind<AbilityDataSO>()
                .FromInstance(destroySpecificFruitAbilityData)
                .WhenInjectedInto<DestroySpecificFruitAbility>();
            
            
            //DestroyItemsAfterContinue ability bind:
            Container
                .Bind<IAbility>()
                .To<DestroyItemsAfterContinue>()
                .AsCached();
                    
            Container
                .Bind<AbilityDataSO>()
                .FromInstance(destroyItemsAfterContinueAbilityData)
                .WhenInjectedInto<DestroyItemsAfterContinue>();
        }
        
        public override void Start()
        {
            var abilities = Container.ResolveAll<IAbility>();

            _abilityManager.InitAbilities(abilities);
        }
        
    }
}