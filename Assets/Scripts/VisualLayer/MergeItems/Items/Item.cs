using DataLayer;
using DataLayer.DataTypes;
using UnityEngine;
using VisualLayer.Factories;
using VisualLayer.MergeItems.MergeSystem;
using Zenject;

namespace VisualLayer.MergeItems
{
    public class Item : MonoBehaviour
    {
        [SerializeField] 
        private ItemMetadata _itemMetadata;
        
        private bool isMerging = false;
        private static float mergeDelay = 0.1f;
        
        [Inject]
        private ItemFactory _itemFactory;
        
        [Inject]
        private IMergeHandler _mergeHandler;
        
        [Inject]
        private IDataLayer _dataLayer;


        [Inject]
        private void Construct(int itemId)
        {
            _itemMetadata = _dataLayer.Metadata.GetItemMetadata(itemId);
        }

        public int GetItemId() => _itemMetadata.ItemId;
        public ItemMetadata GetItemMetadata() => _itemMetadata;

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (isMerging) return;
            
            Item otherItem = other.gameObject.GetComponent<Item>();

            if (otherItem != null && _itemMetadata.ItemId == otherItem._itemMetadata.ItemId)
            {
                HandleCollisionWithSameItem(otherItem);
            }
            else
            {
                HandleCollisionWithJar();
            }
        }

        private void HandleCollisionWithJar()
        {
            gameObject.layer = LayerMask.NameToLayer("StandingFruit");
        }

        private void HandleCollisionWithSameItem(Item otherItem)
        {
            if (_mergeHandler.CanMerge(this, otherItem))
            {
                isMerging = true;
                otherItem.isMerging = true;
                    
                _mergeHandler.Merge(this, otherItem);
            }
        }
    }
}