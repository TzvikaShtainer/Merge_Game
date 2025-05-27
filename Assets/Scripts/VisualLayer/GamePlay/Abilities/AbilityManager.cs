using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DataLayer.DataTypes.abilities;
using ServiceLayer.PlayFabService;
using UnityEngine;
using Zenject;

namespace VisualLayer.GamePlay.Abilities
{
    public class AbilityManager
    {
        public event Action<string, int> OnAbilityChanged;
        
        private Dictionary<string, IAbility> _abilitiesDict = new();

        
        #region Injects
        
        [Inject]
        private IServerService _serverService;
        
        [Inject]
        public void Construct(List<IAbility> abilities)
        {
            //Debug.Log($"Constructing AbilityManager with {abilities?.Count ?? 0} abilities");
            if (abilities == null || abilities.Count == 0)
            {
                //Debug.LogError("No abilities were injected into AbilityManager!");
                return;
            }
    
            _abilitiesDict = abilities.ToDictionary(a => a.Id, a => a);
            //Debug.Log($"Created dictionary with {_abilitiesDict.Count} abilities");
            foreach (var ability in abilities)
            {
                //Debug.Log($"Registered ability: {ability.Id}");
            }

        }
        
        #endregion  

        public void UseAbility(string abilityId)
        {
            if (_abilitiesDict.TryGetValue(abilityId, out var ability))
            {
                ability.UseAbility();

                if (ability.Id == "DestroyItemsAfterContinue")
                {
                    Debug.Log("DestroyItemsAfterContinue");
                    return;
                }
                OnAbilityChanged?.Invoke(abilityId, ability.Count);
                
                _serverService.SetUserData(new Dictionary<string, string>
                {
                    { ability.Id, ability.Count.ToString() }
                }).Forget();
            }
            else
            {
                //Debug.Log($"Ability: {abilityId} - doesn't exist");
            }
        }
        
        public void BuyAbility(string abilityId)
        {
            if (_abilitiesDict.TryGetValue(abilityId, out var ability))
            {
                ability.Buy();
                
                OnAbilityChanged?.Invoke(abilityId, ability.Count);
                
                _serverService.SetUserData(new Dictionary<string, string>
                {
                    {  ability.Id, ability.Count.ToString() }
                }).Forget();
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
        
        public async UniTask LoadFromServer()
        {
            Debug.Log("Loading player abilities from server...");

            var abilityKeys = _abilitiesDict.Keys.ToArray();
            var data = await _serverService.GetUserData(abilityKeys); 
            
            Debug.Log("dict "+_abilitiesDict);
            //אין אתחול
            foreach (var kvp in _abilitiesDict)
            {
                Debug.Log("ssssssss.");
                var id = kvp.Key;
                var ability = kvp.Value;

                Debug.Log(id);
                Debug.Log(ability);
                if (data.TryGetValue(id, out var countStr) && int.TryParse(countStr, out var count))
                {
                    ability.Count = count;
                    OnAbilityChanged?.Invoke(id, count); 
                }
                else
                {
                    Debug.LogWarning($"❗ No data for ability {id}, using default count: {ability.Count}");
                }
            }

            Debug.Log("✅ Finished loading abilities from server.");
        }
    }
}