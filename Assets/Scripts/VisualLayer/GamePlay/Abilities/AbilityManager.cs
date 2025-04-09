using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace VisualLayer.GamePlay.Abilities
{
    public class AbilityManager
    {
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
                ability.Use();
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
            }
        }

        public int GetAbilityCount(string abilityId)
        {
            return _abilitiesDict.TryGetValue(abilityId, out var ability) ? ability.Count : 0;
        }
    }
}