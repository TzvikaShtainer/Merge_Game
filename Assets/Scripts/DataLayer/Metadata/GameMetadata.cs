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
        private ItemsMetadata[] _itemsMetadata;
        
        #endregion
        
        #region Methods
        
        public ItemsMetadata[] GetItemsMetadata() => _itemsMetadata;
        
        public Object GetPrefabForItem(int itemIdToFind)
        {
            var map = _itemsMetadata.FirstOrDefault(c => c.ItemId == itemIdToFind);
            if (map == null)
            {
                throw new InvalidDataException($"Item type {itemIdToFind} in not registered in metadata");
            }

            return map.ItemPrefabRef;
        }
        
        #endregion
    }
}