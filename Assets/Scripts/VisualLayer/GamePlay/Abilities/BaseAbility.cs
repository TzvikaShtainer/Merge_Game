using System.Collections.Generic;
using System.Linq;
using DataLayer;
using DataLayer.DataTypes.abilities;
using ServiceLayer.Signals.SignalsClasses;
using UnityEngine;
using VisualLayer.MergeItems;
using Zenject;

namespace VisualLayer.GamePlay.Abilities
{
    public abstract class BaseAbility : IAbility
    {
        protected AbilityDataSO AbilityDataSo;
        
        protected List<Item> ItemsToToggle;
            
        protected List<Item> AllItems;
        
        public AbilityDataSO Data => AbilityDataSo;
        
        public string Id  => AbilityDataSo.Id;
        public int Count
        {
            get => AbilityDataSo.Count;
            set => AbilityDataSo.Count = Mathf.Max(0, value);
        }
        
        [Inject]
        protected  IDataLayer DataLayer;
        
        [Inject]
        protected  SignalBus SignalBus;
        
        [Inject]
        public void Construct(AbilityDataSO abilityDataSo)
        {
            AbilityDataSo = abilityDataSo;
        }
        
        public virtual void Buy()
        {
            Count++;
            
            DataLayer.Balances.RemoveCoins(AbilityDataSo.Cost);
        }
        
        protected virtual void DisableEnvironment()
        {
            ItemsToToggle = new List<Item>();
            
            AllItems = Object.FindObjectsOfType<Item>().ToList();
            
            SignalBus.Fire<DisableUI>();
            
            SortItems();
            
            DisableItemsOutsideTheJar();
        }
        
        private void SortItems()
        {
            foreach (Item currItem in AllItems)
            {
                if (IsOutsideTheJar(currItem))
                {
                    ItemsToToggle.Add(currItem);
                }
            }
        }

        protected bool IsOutsideTheJar(Item currItem)
        {
            return currItem.transform.position.y >= 2.5f;
        }
        
        private void DisableItemsOutsideTheJar()
        {
            ToggleItems(false);
        }
        
        private void ToggleItems(bool isEnabled)
        {
            foreach (Item currItem in ItemsToToggle)
            {
                currItem.gameObject.SetActive(isEnabled);
            }
        }

        protected void EnableItemsOutsideTheJar()
        {
            ToggleItems(true);
        }
        
        protected bool IsJarEmpty()
        {
            List<Item> allItems = Object.FindObjectsOfType<Item>().ToList();

            //remove items that not inside the jar
            allItems = allItems
                .Where(item => !IsOutsideTheJar(item))
                .ToList();

            if (allItems.Count == 0)
            {
                Debug.Log("No items found");
                return true;
            }

            return false;
        }
        
        public abstract void EnableEnvironment();

        public abstract void UseAbility();
    }
}