using DataLayer;
using Unity.VisualScripting;
using UnityEngine;
using VisualLayer.MergeItems;
using Zenject;

namespace VisualLayer.Factories
{
    public class ItemFactory : PlaceholderFactory<int, Transform, Item>
    {
        
    }

    public class ItemFactoryImplementation : IFactory<int, Transform, Item>
    {
        [Inject]
        private IDataLayer _dataLayer;
        
        [Inject]
        private DiContainer _container;
        
        public Item Create(int param1, Transform param2)
        {
            var itemPrefab = _dataLayer.Metadata.GetPrefabForItem(param1);
            var itemInstance = _container.InstantiatePrefab(itemPrefab, param2);
            
            if (!itemPrefab.GetComponent<Item>())
            {
                throw new System.Exception($"Prefab for item {param1} does not have an Item component!");
            }
            
            return itemInstance.GetComponent<Item>();
        }
    }
}