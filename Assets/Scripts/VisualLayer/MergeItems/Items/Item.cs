using UnityEngine;
using VisualLayer.Factories;
using VisualLayer.MergeItems.MergeSystem;
using Zenject;

namespace VisualLayer.MergeItems
{
    public class Item : MonoBehaviour
    {
        [SerializeField]
        private int _itemId;
        
        private bool isMerging = false;
        private static float mergeDelay = 0.1f;
        
        [Inject]
        private ItemFactory _itemFactory;
        
        [Inject]
        private IMergeHandler _mergeHandler;


        [Inject]
        private void Construct(int itemId)
        {
            _itemId = itemId;
        }

        public int GetItemId() => _itemId;

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (isMerging) return;
            
            Item otherItem = other.gameObject.GetComponent<Item>();

            if (otherItem != null && _itemId == otherItem._itemId)
            {
                if (_mergeHandler.CanMerge(this, otherItem))
                {
                    isMerging = true;
                    otherItem.isMerging = true;
                    
                    _mergeHandler.Merge(this, otherItem);
                }
                
                //MergeItems(otherItem);
            }
        }
    }
}