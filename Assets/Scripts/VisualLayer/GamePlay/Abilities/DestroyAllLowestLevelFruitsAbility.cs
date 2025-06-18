using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DataLayer;
using DataLayer.DataTypes;
using DataLayer.DataTypes.abilities;
using ServiceLayer.EffectsService;
using ServiceLayer.Signals.SignalsClasses;
using ServiceLayer.TimeControl;
using UnityEngine;
using VisualLayer.MergeItems;
using Zenject;
using Object = UnityEngine.Object;

namespace VisualLayer.GamePlay.Abilities
{
    public class DestroyAllLowestLevelFruitsAbility : BaseAbility
    {
        [Inject] 
        private IEffectsManager _effectsManager;

        public override async void UseAbility()
        {
            if (Count <= 0)
                return;
            
            if (IsJarEmpty())
                return;
            
            Count--;
            
            DisableEnvironment();

            FindAndDestroyLowestItems();    
            
            await UniTask.Delay(TimeSpan.FromSeconds(0.5));
            
            EnableEnvironment();
        }
        
        protected override void DisableEnvironment()
        {
            base.DisableEnvironment();
            
            SignalBus.Fire<PauseInputSignal>();
        }

        
        private void FindAndDestroyLowestItems()
        {
            List<Item> allItems = Object.FindObjectsOfType<Item>().ToList();
            
            allItems = RemoveItemsThatNotInTheJar(allItems);

            if (allItems.Count == 0)
            {
                Debug.Log("No items found");
                return;
            }

            int lowestItemIndex = allItems.Min(item => item.GetItemId());

            DestroyLowestItems(allItems, lowestItemIndex);
        }

        private void DestroyLowestItems(List<Item> allItems, int lowestItemIndex)
        {
            for (var index = 0; index < allItems.Count; index++)
            {
                var currItem = allItems[index];
                if (currItem.GetItemId() == lowestItemIndex && !IsOutsideTheJar(currItem))
                {
                    Object.Destroy(currItem.gameObject);
                    _effectsManager.PlayEffect(EffectType.DestroyAbility, currItem.gameObject.transform.position);
                }
            }
        }

        private List<Item> RemoveItemsThatNotInTheJar(List<Item> allItems)
        {
            allItems = allItems
                .Where(item => !IsOutsideTheJar(item))
                .ToList();
            return allItems;
        }
        
        public override void EnableEnvironment()
        {
            SignalBus.Fire<UnpauseInputSignal>();
            SignalBus.Fire<EnableUISignal>();
            
            EnableItemsOutsideTheJar();
        }
    }
}