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
    public enum ItemLayer
    {
        FallingFruit,
        StandingFruit,
        CreatedFruit
    }

    public static class ItemLayerExtensions
    {
        public static int ToLayer(this ItemLayer layer)
        {
            return LayerMask.NameToLayer(layer.ToString());
        }
    }
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
        private CircleCollider2D _circleCollider2D;
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
            gameObject.layer = ItemLayer.CreatedFruit.ToLayer();
            
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
            _circleCollider2D = GetComponent<CircleCollider2D>();
            
            _circleCollider2D.enabled = true;
        }
        

        private void Update()
        {
            if (IsStandingAfterFall())
            {
                //Debug.Log("Standing");
                gameObject.layer = ItemLayer.StandingFruit.ToLayer();
            }
        }

        private bool IsStandingAfterFall()
        {
            if (_rigidbody.linearVelocity.magnitude < 0.1f && _rigidbody.gravityScale != 0)
            {
                if (gameObject.layer == ItemLayer.FallingFruit.ToLayer() && _isLosing)
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

            if (CanMergeItems(otherItem))
            {
                HandleCollisionWithSameItem(otherItem);
            }
            else if (_rigidbody.gravityScale != 0)
            {
                HandleCollisionWithJar();
            }
        }

        private bool CanMergeItems(Item otherItem)
        {
            return otherItem != null
                   && itemMetadata.ItemId == otherItem.itemMetadata.ItemId;
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
            gameObject.layer = ItemLayer.StandingFruit.ToLayer();
        }
        
        public void MakeItemFall(bool enabled)
        {
            _rigidbody.gravityScale = enabled ? 1 : 0;
            
            if ( Mathf.Approximately(_rigidbody.gravityScale, 1))
            {
                _circleCollider2D.enabled = true;
            }
        }
    }
}