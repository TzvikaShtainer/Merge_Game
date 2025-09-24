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

        public void Init(GameLogicHandler handler)
        {
            _gameLogicHandler = handler;
        }
        
        private string SavePath => Path.Combine(Application.persistentDataPath, "GameItemsLocation.json");
        //C:/Users/tzvik/AppData/LocalLow/DreamzStudio/Merge Delicious
        public void Save()
        {
            var itemsToSave = GameObject.FindObjectsOfType<Item>()
                .Select(item => new MergeItemSaveData
                {
                    typeId = item.GetItemId().ToString(),
                    position = SerializableTypes.SerializableVector2.From(item.transform.position),
                    velocity = SerializableTypes.SerializableVector2.From(item.GetComponent<Rigidbody2D>().linearVelocity),
                    rotation = SerializableTypes.SerializableQuaternion.From(item.transform.rotation)
                }).ToList();
            
            var isAbilitiesFirstTime = _abilityManager
                .GetAllAbilityIds()
                .Select(id => new AbilityFirstTimeEntry
                {
                    abilityId = id,
                    isFirstTime = _abilityManager.IsAbilityFirstTime(id)
                })
                .ToList();

            var saveData = new SaveData
            {
                Items = itemsToSave,
                AbilitiesFirstTimeMap = isAbilitiesFirstTime
            };
            
            
            File.WriteAllText(SavePath, JsonUtility.ToJson(saveData, true));
        }

        public async UniTask Load()
        {
            if (!IsFileOk(out var saveData)) return;

            //Debug.Log(json);
            //Debug.Log("SAVEPATH: " + SavePath);

            LoadItemsPosFromData(saveData);

            LoadAbilitiesFirstTimeMap(saveData);
        }
        
        private bool IsFileOk(out SaveData saveData)
        {
            if (!File.Exists(SavePath))
            {
                //Debug.LogWarning("Save file does not exist.");
                saveData = null;
                return false;
            }

            var json = File.ReadAllText(SavePath);

            if (string.IsNullOrWhiteSpace(json))
            {
               // Debug.LogWarning("JSON content is empty or whitespace.");
                saveData = null;
                return false;
            }

            saveData = JsonUtility.FromJson<SaveData>(json);

            if (saveData == null)
            {
               // Debug.LogWarning("Parsed saveData is null.");
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
                _abilityManager.SetFirstTimeFlagFromData(abilityData.abilityId, abilityData.isFirstTime);
            }
        }
    }
}