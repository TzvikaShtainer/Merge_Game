using DataLayer;
using DataLayer.DataTypes;
using ServiceLayer.EffectsService;
using ServiceLayer.Signals.SignalsClasses;
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
        
        [Inject]
        private SignalBus _signalBus;
        
        [Inject]
        private IEffectsManager _effectsManager;
        
        public bool CanMerge(Item item1, Item item2)
        {
            return item1.GetItemId() == item2.GetItemId();
        }

        public void Merge(Item item1, Item item2)
        {
            int fruitNextLevel = item1.GetItemId() + 1;
            
            
            if (HasNextLevel(fruitNextLevel)) 
            {
                _signalBus.Fire<ItemMergedSignal>();
                
                Object.Destroy(item1.gameObject);
                Object.Destroy(item2.gameObject);
            }
            else
            {
                //merge items & destroy Old Items
                MergeItems(item1, item2, fruitNextLevel);
            }
           
            if (fruitNextLevel >= 8)
            {
                _signalBus.Fire<AddCoinsSignal>();
            }
        }

        private bool HasNextLevel(int newLevel)
        {
            return !_dataLayer.Metadata.HasNextLevelItem(newLevel);
        }

        private void MergeItems(Item item1, Item item2, int newLevel)
        {
            //Item newItem = _itemFactory.Create(newLevel,);
            
            Vector2 newPosition = (item1.transform.position + item2.transform.position) / 2;
            Item newItem = _itemFactory.Create(newLevel, newPosition);
           // newItem.transform.position = newPosition;
            
            newItem.gameObject.layer = LayerMask.NameToLayer("StandingFruit");

            FMODUnity.RuntimeManager.PlayOneShot(FModEvents.Instance.Merge, newPosition);
            
            Object.Destroy(item1.gameObject);
            Object.Destroy(item2.gameObject);
            
            _effectsManager.PlayEffect(EffectType.Merge, newPosition);
            
            _signalBus.Fire<ItemMergedSignal>();
        }
    }
}