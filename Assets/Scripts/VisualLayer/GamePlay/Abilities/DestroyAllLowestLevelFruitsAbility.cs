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
            
            List<Item> itemsToToggle = new List<Item>();
            
            List<Item> allItems = Object.FindObjectsOfType<Item>().ToList();
            
            DisableEnvironment(itemsToToggle, allItems);

            FindAndDestroyLowestItems();    
            
            await UniTask.Delay(TimeSpan.FromSeconds(0.5));
            
            EnableEnvironment(itemsToToggle);
        }

        private void DisableEnvironment(List<Item> itemsToToggle, List<Item> allItems)
        {
            _signalBus.Fire<PauseInput>();
            _signalBus.Fire<DisableUI>();
            
            SortItems(itemsToToggle, allItems);
            
            DisableItemsOutsideTheJar(itemsToToggle);
        }
        
        private void SortItems(List<Item> itemsToToggle, List<Item> allItems)
        {
            foreach (Item currItem in allItems)
            {
                if (IsOutsideTheJar(currItem))
                {
                    itemsToToggle.Add(currItem);
                }
            }
        }
        
        private void DisableItemsOutsideTheJar(List<Item> itemsToToggle)
        {
            ToggleItems(itemsToToggle, false);
        }
        
        private void EnableItemsOutsideTheJar(List<Item> itemsToToggle)
        {
            ToggleItems(itemsToToggle, true);
        }

        private void ToggleItems(List<Item> itemsToToggle, bool isEnabled)
        {
            foreach (Item currItem in itemsToToggle)
            {
                currItem.gameObject.SetActive(isEnabled);
            }
        }

        private bool IsOutsideTheJar(Item currItem)
        {
            return currItem.transform.position.y >= 2.5f;
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
        
        private void EnableEnvironment(List<Item> itemsToToggle)
        {
            _signalBus.Fire<UnpauseInput>();
            _signalBus.Fire<EnableUI>();
            
            EnableItemsOutsideTheJar(itemsToToggle);
        }
    }
}