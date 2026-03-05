using System;
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
        public event Action OnRequestExecution;
        
        protected AbilityDataSO AbilityDataSo;
        protected bool IsFirstTimeUse = false;
        public AbilityDataSO Data => AbilityDataSo;
        public string Id  => AbilityDataSo.Id;
        public int Count
        {
            get => AbilityDataSo.Count;
            set => AbilityDataSo.Count = Mathf.Max(0, value);
        }
        
        public void TriggerRequest()
        {
            OnRequestExecution?.Invoke();
        }
        
        protected  LazyInject<IDataLayer> DataLayer;
        
        protected BaseAbility(AbilityDataSO abilityDataSo, LazyInject<IDataLayer> dataLayer)
        {
            AbilityDataSo = abilityDataSo;
            DataLayer = dataLayer;
        }
        
        public virtual void Buy()
        {
            Count++;
            
            DataLayer.Value.Balances.RemoveCoins(AbilityDataSo.Cost);
        }

        public void AddAbilityCount(int amountToAdd)
        {
            Count += amountToAdd;
        }

        public bool IsFirstTime()
        {
            return IsFirstTimeUse;
        }
        
        public virtual void UseAbility()
        {
            if (Count <= 0) return;
            
            IsFirstTimeUse = true;
            OnRequestExecution?.Invoke();
        }
        
        public void SetFirstTime(bool value)
        {
            IsFirstTimeUse = value;
        }
    }
}