using DataLayer.Balances;
using DataLayer.DataTypes;
using DataLayer.Metadata;
using Unity.VisualScripting;
using UnityEngine;
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
        
        
        public override void InstallBindings()
        {
            Container
                .Bind<IDataLayer>()
                .FromSubContainerResolve()
                .ByMethod(SubContainerBindings)
                .AsSingle();
        }

        private void SubContainerBindings(DiContainer subContainer)
        {
            subContainer
                .Bind<IDataLayer>()
                .To<DataLayer>()
                .AsSingle();
            
            subContainer
                .Bind<IPlayerBalances>()
                .To<PlayerBalances>()
                .FromInstance(_playerBalances )
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
        }
    }
}