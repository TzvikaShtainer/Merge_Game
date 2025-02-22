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
        
        public Item Create(int param1)
        {
            var itemPrefab = _dataLayer.Metadata.GetPrefabForItem(param1);
            var itemInstance = _container.InstantiatePrefab(itemPrefab);
            
            if (!itemPrefab.GetComponent<Item>())
            {
                throw new System.Exception($"Prefab for item {param1} does not have an Item component!");
            }
            
            return itemInstance.GetComponent<Item>();
        }
    }
}