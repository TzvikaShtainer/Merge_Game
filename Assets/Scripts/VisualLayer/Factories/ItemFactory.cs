using DataLayer;
using Unity.VisualScripting;
using UnityEngine;
using VisualLayer.MergeItems;
using Zenject;

namespace VisualLayer.Factories
{
    public class ItemFactory : PlaceholderFactory<int, Item>
    {
        
    }

    public class ItemFactoryImplementation : IFactory<int, Item>
    {
        [Inject]
        private IDataLayer _dataLayer;
        
        [Inject]
        private DiContainer _container;
        
        public Item Create(int itemId)
        {
            var itemPrefab = _dataLayer.Metadata.GetPrefabForItem(itemId);
            Item instanceToReturn = _container.InstantiatePrefabForComponent<Item>(itemPrefab, new object[] { itemId });
            
            if (!instanceToReturn)
            {
                throw new System.Exception($"Prefab for item {itemId} does not have an Item component!");
            }
            
            
            return instanceToReturn;
        }
    }
}