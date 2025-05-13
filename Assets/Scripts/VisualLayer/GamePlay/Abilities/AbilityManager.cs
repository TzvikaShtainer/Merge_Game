using System;
using System.Collections.Generic;
using System.Linq;
using DataLayer.DataTypes.abilities;
using UnityEngine;
using Zenject;

namespace VisualLayer.GamePlay.Abilities
{
    public class AbilityManager
    {
        public event Action<string, int> OnAbilityChanged;
        
        private Dictionary<string, IAbility> _abilitiesDict = new();

        
        [Inject]
        public void Construct(List<IAbility> abilities)
        {
            _abilitiesDict = abilities.ToDictionary(a => a.Id, a => a);
        }

        public void UseAbility(string abilityId)
        {
            if (_abilitiesDict.TryGetValue(abilityId, out var ability))
            {
                ability.UseAbility();
                
                OnAbilityChanged?.Invoke(abilityId, ability.Count);
            }
            else
            {
                Debug.Log($"Ability: {abilityId} - doesn't exist");
            }
        }
        
        public void BuyAbility(string abilityId)
        {
            if (_abilitiesDict.TryGetValue(abilityId, out var ability))
            {
                ability.Buy();
                
                OnAbilityChanged?.Invoke(abilityId, ability.Count);
            }
        }

        public int GetAbilityCount(string abilityId)
        {
            return _abilitiesDict.TryGetValue(abilityId, out var ability) ? ability.Count : 0;
        }

        public AbilityDataSO GetAbilitySO(string abilityId)
        {
            return _abilitiesDict.TryGetValue(abilityId, out var ability) 
                ? ability.Data 
                : null;
        }
    }
}