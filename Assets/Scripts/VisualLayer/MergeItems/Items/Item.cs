using UnityEngine;
using Zenject;

namespace VisualLayer.MergeItems
{
    public class Item : MonoBehaviour
    {
        [SerializeField]
        private int _itemId;
        
        private bool isMerging = false;


        [Inject]
        private void Construct(int itemId)
        {
            _itemId = itemId;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (isMerging) return;
            
            Item otherItem = other.gameObject.GetComponent<Item>();

            if (otherItem != null && _itemId == otherItem._itemId)
            {
                Debug.Log("merge");
                MergeItems(otherItem);
            }
        }

        private void MergeItems(Item otherItem)
        {
            isMerging = true;
            otherItem.isMerging = true;
            
            
        }
    }
}