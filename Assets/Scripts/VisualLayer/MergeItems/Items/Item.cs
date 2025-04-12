using System;
using DataLayer;
using DataLayer.DataTypes;
using ServiceLayer.Signals.SignalsClasses;
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
        private Rigidbody2D _rigidbody;
        private bool _isLosing = false;
        
        [Inject]
        private ItemFactory _itemFactory;
        
        [Inject]
        private IMergeHandler _mergeHandler;
        
        [Inject]
        private IDataLayer _dataLayer;
        
        [Inject]
        private SignalBus _signalBus;
      

        [Inject]
        private void Construct(int itemId)
        {
            _itemMetadata = _dataLayer.Metadata.GetItemMetadata(itemId);
            _signalBus.Subscribe<HandleItemsCollisionAfterLose>(OnPlayerLose);
        }

        private void OnPlayerLose()
        {
            
            _isLosing = true;
            gameObject.layer = LayerMask.NameToLayer("CreatedFruit");
        }

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }
        

        private void Update()
        {
            if (IsStandingAfterFall())
            {
                //Debug.Log("Standing");
                gameObject.layer = LayerMask.NameToLayer("StandingFruit");
            }
        }

        private bool IsStandingAfterFall()
        {
            if (_rigidbody.velocity.magnitude < 0.1f)
            {
                if (gameObject.layer == LayerMask.NameToLayer("FallingFruit") && _isLosing)
                {
                    //Debug.Log("IsStandingAfterFall true");
                    return true;
                }
            }
            return false;
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
        
        private void HandleCollisionWithSameItem(Item otherItem)
        {
            if (_mergeHandler.CanMerge(this, otherItem))
            {
                isMerging = true;
                otherItem.isMerging = true;
                    
                _mergeHandler.Merge(this, otherItem);
            }
        }
        
        private void HandleCollisionWithJar()
        {
            gameObject.layer = LayerMask.NameToLayer("StandingFruit");
        }
    }
}