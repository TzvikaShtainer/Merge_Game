using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DataLayer.DataTypes.abilities;
using ServiceLayer.DataSyncService;
using ServiceLayer.PlayFabService;
using UnityEngine;
using Zenject;

namespace VisualLayer.GamePlay.Abilities
{
    public class AbilityManager : ISyncableService
    {
        public event Action OnDataChanged;
        public event Action<string, int> OnAbilityChanged;
        
        private Dictionary<string, IAbility> _abilitiesDict = new();
        
        #region Injects
        
        [Inject]
        private IServerService _serverService;
        
        public Dictionary<string, string> GetSyncData()
        {
            var data = new Dictionary<string, string>();
            foreach (var pair in _abilitiesDict)
            {
                if (pair.Key == "DestroyItemsAfterContinue")
                    continue;
                
                data[pair.Key] = pair.Value.Count.ToString();
            }
            
            Debug.Log($"[AbilityManager] GetSyncData returning {data.Count} abilities");
    
            return data;
        }
        
        public void InitAbilities(List<IAbility> abilities)
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
                    //Debug.Log("DestroyItemsAfterContinue");
                    return;
                }
                OnAbilityChanged?.Invoke(abilityId, ability.Count);
                
                OnDataChanged?.Invoke();
                
                // _serverService.SetUserData(new Dictionary<string, string>
                // {
                //     { ability.Id, ability.Count.ToString() }
                // }).Forget();
            }
        }
        
        public void BuyAbility(string abilityId)
        {
            if (_abilitiesDict.TryGetValue(abilityId, out var ability))
            {
                ability.Buy();
                
                OnAbilityChanged?.Invoke(abilityId, ability.Count);
                
                OnDataChanged?.Invoke();
                
                // _serverService.SetUserData(new Dictionary<string, string>
                // {
                //     {  ability.Id, ability.Count.ToString() }
                // }).Forget();
            }

            //Debug.LogError("SetFirstTimeFlagFromData called 81");
            SetFirstTimeFlagFromData(abilityId, true);
        }

        public async void AddAbilityCount(string abilityId, int amountToAdd)
        {
            if (string.IsNullOrEmpty(abilityId))
            {
                Debug.LogError("AddAbilityCount called with null or empty abilityId");
                return;
            }

            if (_abilitiesDict.TryGetValue(abilityId, out var ability))
            {
                ability.Count += amountToAdd;
                
                OnAbilityChanged?.Invoke(abilityId, ability.Count);
                SetFirstTimeFlagFromData(abilityId, true);
                
                OnDataChanged?.Invoke();
            }
            else
            {
                Debug.LogWarning($"[AbilityManager] Attempted to add count to unknown ability: {abilityId}");
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
            //Debug.Log("Loading player abilities from server...");

            var abilityKeys = _abilitiesDict.Keys.ToArray();
            var data = await _serverService.GetUserData(abilityKeys); 
            
            //Debug.Log($"Total keys: {_abilitiesDict.Keys.Count}");
            
            foreach (var kvp in _abilitiesDict)
            {
                //Debug.Log($"Inside Dict: {kvp.Key}.");
                var id = kvp.Key;
                var ability = kvp.Value;

                //Debug.Log(id);
                //Debug.Log(ability);
                if (data.TryGetValue(id, out var countStr) && int.TryParse(countStr, out var count))
                {
                    ability.Count = count;
                    OnAbilityChanged?.Invoke(id, count); 
                }
                else
                {
                    //Debug.LogWarning($"❗ No data for ability {id}, using default count: {ability.Count}");
                }
            }

            //Debug.Log("✅ Finished loading abilities from server.");
        }
        public IEnumerable<string> GetAllAbilityIds()
        {
            return _abilitiesDict.Keys;
        }
        public bool IsAbilityFirstTime(string abilityId)
        {
            return _abilitiesDict.TryGetValue(abilityId, out var ability) && ability.IsFirstTime();
        }

        public void SetFirstTimeFlagFromData(string abilityId, bool value)
        {
            if (_abilitiesDict.TryGetValue(abilityId, out var ability))
            {
                ability.SetFirstTime(value);
                //Debug.Log("SetFirstTimeFlagFromData: ability: "+abilityId +"value: "+value);
            }
            else
            {
               // Debug.LogWarning($"Tried to set first-time flag for unknown ability: {abilityId}");
            }
        }
    }
}