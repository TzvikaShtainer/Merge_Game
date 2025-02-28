using DataLayer;
using UnityEngine;
using VisualLayer.Factories;
using Zenject;

namespace VisualLayer.MergeItems.MergeSystem
{
    public class MergeHandler : IMergeHandler
    {
        [Inject]
        private IDataLayer _dataLayer;
        
        [Inject]
        private ItemFactory _itemFactory;
        
        public bool CanMerge(Item item1, Item item2)
        {
            return item1.GetItemId() == item2.GetItemId();
        }

        public void Merge(Item item1, Item item2)
        {
            int newLevel = item1.GetItemId() + 1;
            
            if (!_dataLayer.Metadata.HasNextLevelItem(newLevel))
            {
                //notify for coins and destory highest merge
            }
            
            //merge items
            Item newItem = _itemFactory.Create(newLevel);
            
            Vector2 newPosition = (item1.transform.position + item2.transform.position) / 2;
            newItem.transform.position = newPosition;
            
            Object.Destroy(item1.gameObject);
            Object.Destroy(item2.gameObject);
        }
    }
}