using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
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
    public class DestroyAllLowestLevelFruitsAbility : IAbility
    {
        [Inject]
        private SignalBus _signalBus;
        
        [Inject] 
        private IEffectsManager _effectsManager;
        
        public string Id => _abilityDataSo.Id;
        public int Count
        {
            get => _abilityDataSo.Count;
            set => _abilityDataSo.Count = value;
        }
        
        private AbilityDataSO _abilityDataSo;

        [Inject]
        public void Construct(AbilityDataSO abilityDataSo)
        {
            _abilityDataSo = abilityDataSo;
        }
        
        public void Buy()
        {
            throw new System.NotImplementedException();
        }
        
        public async void UseAbility()
        {
            if (Count <= 0)
                return;
            
            Count--;
            _signalBus.Fire<PauseInput>();
            _signalBus.Fire<DisableUI>();

            FindAndDestroyLowestItems();    
            
            await UniTask.Delay(TimeSpan.FromSeconds(0.5));
            _signalBus.Fire<UnpauseInput>();
            _signalBus.Fire<EnableUI>();
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
                    _effectsManager.PlayEffect(EffectType.Destroy, currItem.gameObject.transform.position);
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

        private bool IsOutsideTheJar(Item currItem)
        {
            return currItem.gameObject.transform.position.y >= 2.5;
        }
    }
}