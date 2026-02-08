using DataLayer.Balances;
using DataLayer.DataTypes;
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
        
        public override void InstallBindings()
        {
            Container
                .Bind<SfxDatabase>()
                .FromInstance(_sfxDatabase)
                .AsSingle();
            
            Container
                .Bind<EffectsDatabase>()
                .FromInstance(_effectsDatabase)
                .AsSingle();

            foreach (var currEffect in _effectsDatabase.effects)
            {
                //Container.BindMemoryPool<EffectPoolItem, EffectPool>()
                //    .WithId(currEffect.effectType)
                //    .FromComponentInNewPrefab(currEffect.particleSystem.gameObject)
                //    .UnderTransformGroup("EffectPool");
                
                Container
                    .BindFactory<EffectPoolItem, EffectPoolItem.Factory>()
                    .WithId(currEffect.effectType)
                    .FromPoolableMemoryPool(
                        poolInitializer => poolInitializer
                            .WithInitialSize(1)
                            .WithMaxSize(5)
                            .FromComponentInNewPrefab(currEffect.particleSystem)
                            .UnderTransformGroup("Effects Pool"));
            }
            
            Container
                .Bind<IDataLayer>()
                .FromSubContainerResolve()
                .ByMethod(SubContainerBindings)
                .AsSingle();
            
            Container.Bind<ISyncableService>()
                .FromMethod(ctx => {
                    var dataLayer = ctx.Container.Resolve<IDataLayer>();
                    return dataLayer.Balances as ISyncableService;
                })
                .AsCached();
        }

        private void SubContainerBindings(DiContainer subContainer)
        {
            subContainer
                .Bind<IDataLayer>()
                .To<DataLayer>()
                .AsSingle();
            
            var serverService = Container.Resolve<IServerService>();

            _playerBalances.Initialize(serverService);
            
            subContainer
                .Bind(typeof(IPlayerBalances), typeof(ISyncableService))
                .To<PlayerBalances>()
                .FromInstance(_playerBalances)
                .AsSingle();
            
            subContainer
                .Bind<IGameMetadata>()
                .To<GameMetadata>()
                .AsSingle();

            subContainer
                .Bind<ItemMetadata[]>()
                .FromInstance(_items)
                .AsSingle();
            
            subContainer
                .Bind<GameLevelMetadata[]>()
                .FromInstance(_levelsMetadata)
                .AsCached();
            
            subContainer
                .Bind<InfraScreenMetadata[]>()
                .FromInstance(_infraScreenMetadatas)
                .AsCached();
            
            Container
                .Bind<AbilityManager>()
                .AsSingle();
            
            Container
                .BindInstance(dailyRewardsConfig)
                .AsSingle();
            
            
        }
    }
}