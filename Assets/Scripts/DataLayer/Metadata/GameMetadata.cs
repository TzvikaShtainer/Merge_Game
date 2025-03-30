using System.IO;
using System.Linq;
using DataLayer.DataTypes;
using UnityEngine;
using Zenject;

namespace DataLayer.Metadata
{
    public class GameMetadata : IGameMetadata
    {
        #region Injects
        
        [Inject]
        private ItemMetadata[] _itemsMetadata;
        
        [Inject]
        private GameLevelMetadata[] _levelsMetadata;

        [Inject]
        private InfraScreenMetadata[] _infraScreenMetadata;
        
        #endregion
        
        #region Methods
        
        public ItemMetadata[] GetItemsMetadata() => _itemsMetadata;
        
        public Object GetPrefabForItem(int itemIdToFind)
        {
            var map = _itemsMetadata.FirstOrDefault(c => c.ItemId == itemIdToFind);
            if (map == null)
            {
                throw new InvalidDataException($"Item type {itemIdToFind} in not registered in metadata");
            }

            return map.ItemPrefabRef;
        }

        public bool HasNextLevelItem(int nextItemId)
        {
            
            if (nextItemId < 0 || nextItemId >= _itemsMetadata.Length)
            {
                return false;
            }
            
            
            ItemMetadata itemMetadata = _itemsMetadata[nextItemId];
            return itemMetadata != null;
        }

        public ItemMetadata GetItemMetadata(int itemIdToFind)
        {
            var map = _itemsMetadata.FirstOrDefault(c => c.ItemId == itemIdToFind);
            if (map == null)
            {
                throw new InvalidDataException($"Item type {itemIdToFind} in not registered in metadata");
            }

            return map;
        }
        
        public GameLevelMetadata GetLevelMetadata(GameLevelType levelType)
        {
            var result = _levelsMetadata.FirstOrDefault(m => m.LevelType == levelType);
            if (result == null)
            {
                throw new InvalidDataException($"{levelType} level isn't supported");
            }

            return result;
        }

        public InfraScreenMetadata GetInfraScreenMetadata(InfraScreenType screenType)
        {
            var result = _infraScreenMetadata.FirstOrDefault(m => m.Type == screenType);
            if (result == null)
            {
                throw new InvalidDataException($"{screenType} infra screen isn't supported");
            }

            return result;
        }
        
        #endregion
    }
}