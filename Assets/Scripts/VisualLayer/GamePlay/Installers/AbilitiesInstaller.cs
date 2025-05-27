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

        public override void InstallBindings()
        {
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
}