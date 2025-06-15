using DataLayer;
using Unity.VisualScripting;
using UnityEngine;
using VisualLayer.MergeItems;
using Zenject;

namespace VisualLayer.Factories
{
    public class ItemFactory : PlaceholderFactory<int, Vector2, Item>
    {
        
    }

    public class ItemFactoryImplementation : IFactory<int, Vector2, Item>
    {
        [Inject]
        private IDataLayer _dataLayer;
        
        [Inject]
        private DiContainer _container;
        
        private int _creationCounter = 0;
        
        public Item Create(int itemId, Vector2 pos)
        {
            var itemPrefab = _dataLayer.Metadata.GetPrefabForItem(itemId);
            
            //Item instanceToReturn = _container.InstantiatePrefabForComponent<Item>(itemPrefab, new object[] { itemId });
            Item instanceToReturn = _container.InstantiatePrefabForComponent<Item>(
                itemPrefab,
                pos,
                Quaternion.identity,
                null, 
                new object[] { itemId }
            );
            
            if (!instanceToReturn)
            {
                throw new System.Exception($"Prefab for item {itemId} does not have an Item component!");
            }
            
            _creationCounter++; 
            instanceToReturn.name = $"Item_{_creationCounter}"; 
            
            return instanceToReturn;
        }
    }
}