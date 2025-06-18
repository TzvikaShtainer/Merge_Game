using System;
using DataLayer;
using DataLayer.DataTypes;
using ServiceLayer.Signals.SignalsClasses;
using UnityEngine;
using UnityEngine.Serialization;
using VisualLayer.Factories;
using VisualLayer.MergeItems.MergeSystem;
using Zenject;

namespace VisualLayer.MergeItems
{
    public class Item : MonoBehaviour
    {
        [SerializeField] 
        private ItemMetadata itemMetadata;

        [SerializeField]
        private SpriteRenderer itemSprite;
        
        [Inject]
        private IMergeHandler _mergeHandler;
        
        [Inject]
        private IDataLayer _dataLayer;
        
        [Inject]
        private SignalBus _signalBus;
        
        private bool _isMerging = false;
        private Rigidbody2D _rigidbody;
        private bool _isLosing = false;
      

        [Inject]
        private void Construct(int itemId)
        {
            itemMetadata = _dataLayer.Metadata.GetItemMetadata(itemId);
            _signalBus.Subscribe<HandleItemsCollisionAfterLoseSignal>(OnPlayerLose);
            _signalBus.Subscribe<OnContinueClickedSignal>(OnPlayerContinueClicked);
        }

        private void OnPlayerLose()
        {
            if (this == null) return;
            
            _isLosing = true;
            gameObject.layer = LayerMask.NameToLayer("CreatedFruit");
            
            itemSprite.sprite = itemMetadata.ItemSadSprite;
        }

        private void OnPlayerContinueClicked()
        {
            if (this == null) return;
            
            _isLosing = false;
            itemSprite.sprite = itemMetadata.ItemPreviewSprite;
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
            if (_rigidbody.linearVelocity.magnitude < 0.1f)
            {
                if (gameObject.layer == LayerMask.NameToLayer("FallingFruit") && _isLosing)
                {
                    //Debug.Log("IsStandingAfterFall true");
                    return true;
                }
            }
            return false;
        }

        public int GetItemId() => itemMetadata.ItemId;
        public ItemMetadata GetItemMetadata() => itemMetadata;

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (_isMerging) return;
            
            Item otherItem = other.gameObject.GetComponent<Item>();

            if (otherItem != null && itemMetadata.ItemId == otherItem.itemMetadata.ItemId)
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
                _isMerging = true;
                otherItem._isMerging = true;
                    
                _mergeHandler.Merge(this, otherItem);
            }
        }
        
        private void HandleCollisionWithJar()
        {
            gameObject.layer = LayerMask.NameToLayer("StandingFruit");
        }
    }
}