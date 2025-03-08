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
            ItemMetadata itemMetadata = _itemsMetadata[nextItemId];

            if (itemMetadata != null)
            {
                return true;
            }

            return false;
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
        
        #endregion
    }
}