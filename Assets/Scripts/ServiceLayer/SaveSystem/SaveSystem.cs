using System.Collections.Generic;
using System.IO;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VisualLayer.GamePlay.Abilities;
using VisualLayer.GamePlay.Handlers;
using VisualLayer.MergeItems;
using Zenject;

namespace ServiceLayer.SaveSystem
{
    public class SaveSystem : ISaveSystem
    {
        [Inject]
        private AbilityManager  _abilityManager;
        
        private GameLogicHandler _gameLogicHandler;
        
        private bool _isReadyToSave = false;

        public void Init(GameLogicHandler handler)
        {
            _gameLogicHandler = handler;
        }

        public async UniTask ClearSave()
        {
            try
            {
                _isReadyToSave = false;

                if (File.Exists(SavePath))
                {
                    File.Delete(SavePath);
                    Debug.Log("[SaveSystem] Save file deleted.");
                }
        
                await UniTask.CompletedTask;
            }
            catch (System.Exception e)
            {
                Debug.LogError($"[SaveSystem] ClearSave failed: {e.Message}");
            }
        }

        private string SavePath => Path.Combine(Application.persistentDataPath, "GameItemsLocation.json");
        //C:\Users\tzvik\AppData\LocalLow\DreamzzzStudio\Merge Delicious
        public async UniTask Save()
        {
            if (!_isReadyToSave)
            {
                Debug.Log("Cant SAve now"+_isReadyToSave);
                return;
            }
            
            //Debug.Log($"Saving {SavePath}");
            Dictionary<string, bool> existingFlags = new Dictionary<string, bool>();
            if (File.Exists(SavePath))
            {
                try
                {
                    string json = File.ReadAllText(SavePath);
                    if (!string.IsNullOrWhiteSpace(json))
                    {
                        var existingData = JsonUtility.FromJson<SaveData>(json);
                        if (existingData != null && existingData.AbilitiesFirstTimeMap != null)
                        {
                            foreach (var entry in existingData.AbilitiesFirstTimeMap)
                            {
                                existingFlags[entry.abilityId] = entry.isFirstTime;
                            }
                        }
                    }
                }
                catch (System.Exception e)
                {
                    Debug.LogWarning($"Failed to read existing save file during Save merge: {e.Message}");
                }
            }

            // Use FindObjectsOfType with includeInactive=true to ensure we save items even if they are temporarily disabled (e.g. by an ability)
            var itemsToSave = Object.FindObjectsOfType<Item>(true)
                .Select(item => new MergeItemSaveData
                {
                    typeId = item.GetItemId().ToString(),
                    position = SerializableTypes.SerializableVector2.From(item.transform.position),
                    velocity = SerializableTypes.SerializableVector2.From(item.GetComponent<Rigidbody2D>().linearVelocity),
                    rotation = SerializableTypes.SerializableQuaternion.From(item.transform.rotation)
                }).ToList();
            
            //Debug.Log($"[SaveSystem] Saving {itemsToSave.Count} items.");
            // if (itemsToSave.Count == 0)
            // {
            //     Debug.LogWarning("[SaveSystem] WARNING: Saving 0 items! This will result in an empty board on load.");
            // }

            var isAbilitiesFirstTime = _abilityManager
                .GetAllAbilityIds()
                .Select(id => 
                {
                    bool currentVal = _abilityManager.IsAbilityFirstTime(id);
                    if (!currentVal && existingFlags.TryGetValue(id, out bool savedVal) && savedVal)
                    {
                        //Debug.Log($"[SaveSystem] Preserving TRUE state for ability {id} despite current memory being FALSE.");
                        return new AbilityFirstTimeEntry { abilityId = id, isFirstTime = true };
                    }
                    
                    return new AbilityFirstTimeEntry { abilityId = id, isFirstTime = currentVal };
                })
                .ToList();

            //Debug.Log($"<color=cyan>Finalizing Save: Logging {isAbilitiesFirstTime.Count} Abilities First Time Status:</color>");
            // foreach (var entry in isAbilitiesFirstTime)
            // {
            //     Debug.Log($"[Ability Save] ID: {entry.abilityId}, IsFirstTime: {entry.isFirstTime}");
            // }
            
            var saveData = new SaveData
            {
                Items = itemsToSave,
                AbilitiesFirstTimeMap = isAbilitiesFirstTime
            };
            
            
            File.WriteAllText(SavePath, JsonUtility.ToJson(saveData, true));
            //Debug.Log($"Saved successfully to: {SavePath}");
        }

        public async UniTask Load()
        {
            _isReadyToSave = false;
            Debug.Log("Im loading "+_isReadyToSave);
            
            if (!IsFileOk(out var saveData))
            {
                _isReadyToSave = true; 
                Debug.Log("im ready to save "+_isReadyToSave);
                return;
            }

            //Debug.Log(json);
            //Debug.Log("SAVEPATH: " + SavePath);

            LoadItemsPosFromData(saveData);

            LoadAbilitiesFirstTimeMap(saveData);
        }
        
        private bool IsFileOk(out SaveData saveData)
        {
            if (!File.Exists(SavePath))
            {
                //Debug.LogWarning($"Save file does not exist at: {SavePath}");
                saveData = null;
                return false;
            }

            var json = File.ReadAllText(SavePath);

            if (string.IsNullOrWhiteSpace(json))
            {
                //Debug.LogWarning("JSON content is empty or whitespace.");
                saveData = null;
                return false;
            }

            try 
            {
                saveData = JsonUtility.FromJson<SaveData>(json);
            }
            catch (System.Exception e)
            {
                //Debug.LogError($"Failed to parse JSON: {e.Message}");
                saveData = null;
                return false;
            }

            if (saveData == null)
            {
                //Debug.LogWarning("Parsed saveData is null.");
                return false;
            }

            if (saveData.Items == null || saveData.Items.Count == 0)
            {
                //Debug.LogWarning("No items found in save data.");
                return false;
            }

            if (saveData.AbilitiesFirstTimeMap == null)
            {
                //Debug.LogWarning("No AbilitiesFirstTimeMap found in save data.");
                return false;
            }
            
            return true;
        }
        
        private void LoadItemsPosFromData(SaveData saveData)
        {
            foreach (var itemData in saveData.Items)
            {
                if (itemData.position.y == 2.5f || itemData.position.y == 10f)
                {
                    //Debug.Log("Skipping item at position y=2.5 or y=10");
                    continue;
                }

                //Debug.Log("create new item");
                var item = _gameLogicHandler.CreateItemFromSave(itemData.typeId, itemData.position);
                item.transform.rotation = itemData.rotation.ToQuaternion();
                item.GetComponent<Rigidbody2D>().linearVelocity = itemData.velocity.ToVector2();
                item.MakeItemFall(true);
            }
        }
        
        private void LoadAbilitiesFirstTimeMap(SaveData saveData)
        {
            foreach (var abilityData in saveData.AbilitiesFirstTimeMap)
            {
               // Debug.Log("LoadAbilitiesFirstTimeMap abilityId " +abilityData.abilityId + "isFirstTime " +abilityData.isFirstTime);
                _abilityManager.SetFirstTimeFlagFromData(abilityData.abilityId, abilityData.isFirstTime);
            }
        }
    }
}