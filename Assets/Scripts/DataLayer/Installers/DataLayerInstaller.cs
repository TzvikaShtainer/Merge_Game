using System;
using System.Collections.Generic;
using DataLayer.Balances;
using DataLayer.DataTypes;
using DataLayer.DataTypes.abilities;
using DataLayer.Metadata;
using ServiceLayer.DataSyncService;
using ServiceLayer.EffectsService;
using ServiceLayer.MusicService;
using ServiceLayer.PlayFabService;
using Unity.VisualScripting;
using UnityEngine;
using VisualLayer.GamePlay.Abilities;
using VisualLayer.GamePlay.RewardSystem;
using Zenject;

namespace DataLayer.Installers
{
    [CreateAssetMenu(menuName = "Merge/Data/Data Layer Installer", fileName = "DataLayer Installer")]
    public class DataLayerInstaller : ScriptableObjectInstaller<DataLayerInstaller>
    {
        [SerializeField]
        private ItemMetadata[] _items;
        
        [SerializeField] 
        private PlayerBalances _playerBalances;
        
        [SerializeField]
        private GameLevelMetadata[] _levelsMetadata;
        
        [SerializeField]
        private InfraScreenMetadata[] _infraScreenMetadatas;

        [SerializeField] 
        private EffectsDatabase _effectsDatabase;
        
        [SerializeField] 
        private SfxDatabase _sfxDatabase;
        
        [SerializeField]
        public DailyRewardsConfig dailyRewardsConfig;
        
        [Header("Abilities Settings")]
        [Header("Destroy Lowest Ability")]
        [SerializeField]
        private AbilityDataSO destroyLowestAbilityData;
    
        [Header("Shake Box Ability")]
        [SerializeField]
        private AbilityDataSO shakeBoxAbilityData;
        
        [Header("Upgrade Specific FruitAbility")]
        [SerializeField]
        private AbilityDataSO upgradeSpecificFruitAbilityData ;
    
        [Header("Destroy Specific Fruit Ability")]
        [SerializeField]
        private AbilityDataSO destroySpecificFruitAbilityData;
        
        [Header("Destroy Items After Continue Ability")]
        [SerializeField]
        private AbilityDataSO destroyItemsAfterContinueAbilityData;
        
       public override void InstallBindings()
        {
            Container.Bind<SfxDatabase>().FromInstance(_sfxDatabase).AsSingle();
            Container.Bind<EffectsDatabase>().FromInstance(_effectsDatabase).AsSingle();

            foreach (var currEffect in _effectsDatabase.effects)
            {
                Container
                    .BindFactory<EffectPoolItem, EffectPoolItem.Factory>()
                    .WithId(currEffect.effectType)
                    .FromPoolableMemoryPool(poolInitializer => poolInitializer
                        .WithInitialSize(1)
                        .WithMaxSize(5)
                        .FromComponentInNewPrefab(currEffect.particleSystem)
                        .UnderTransformGroup("Effects Pool"));
            }

            Container
                .Bind(typeof(IDataLayer), typeof(AbilityManager), typeof(List<IAbility>))
                .FromSubContainerResolve()
                .ByMethod(SubContainerBindings)
                .AsSingle();

            Container.Bind<ISyncableService>()
                .FromMethod(ctx => ctx.Container.Resolve<IDataLayer>().Balances as ISyncableService)
                .AsCached();

            Container.Bind<ISyncableService>()
                .FromMethod(ctx => ctx.Container.Resolve<AbilityManager>() as ISyncableService)
                .AsCached();
            
            Container.BindInstance(dailyRewardsConfig).AsSingle();
        }

        private void SubContainerBindings(DiContainer subContainer)
        {
            // Binding יחיד לכל סוג בתוך התת-קונטיינר
            subContainer.Bind<IDataLayer>().To<DataLayer>().AsSingle();
            subContainer.Bind<AbilityManager>().AsSingle();

            var serverService = Container.Resolve<IServerService>();
            _playerBalances.Initialize(serverService);

            subContainer
                .Bind(typeof(IPlayerBalances), typeof(ISyncableService))
                .To<PlayerBalances>()
                .FromInstance(_playerBalances)
                .AsSingle();

            subContainer.Bind<IGameMetadata>().To<GameMetadata>().AsSingle();
            subContainer.Bind<ItemMetadata[]>().FromInstance(_items).AsSingle();
            subContainer.Bind<GameLevelMetadata[]>().FromInstance(_levelsMetadata).AsCached();
            subContainer.Bind<InfraScreenMetadata[]>().FromInstance(_infraScreenMetadatas).AsCached();
            

            // --- רישום יכולות עם WithArguments (הזרקה בטוחה ל-Constructor) ---
            

            subContainer.Bind<IAbility>().To<DestroyAllLowestLevelFruitsAbility>()
                .AsCached().WithArguments(destroyLowestAbilityData);

            subContainer.Bind<IAbility>().To<ShakeBoxAbility>()
                .AsCached().WithArguments(shakeBoxAbilityData);

            subContainer.Bind<IAbility>().To<UpgradeSpecificFruitAbility>()
                .AsCached().WithArguments(upgradeSpecificFruitAbilityData);

            subContainer.Bind<IAbility>().To<DestroySpecificFruitAbility>()
                .AsCached().WithArguments(destroySpecificFruitAbilityData);

            subContainer.Bind<IAbility>().To<DestroyItemsAfterContinue>()
                .AsCached().WithArguments(destroyItemsAfterContinueAbilityData);
        }
    }
}