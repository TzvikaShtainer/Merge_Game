using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DataLayer.DataTypes;
using DataLayer.DataTypes.abilities;
using ServiceLayer.EffectsService;
using ServiceLayer.Signals.SignalsClasses;
using UnityEngine;
using VisualLayer.Components.UI;
using VisualLayer.Factories;
using VisualLayer.GamePlay.PlayerInput;
using VisualLayer.MergeItems;
using Zenject;
using Object = UnityEngine.Object;

namespace VisualLayer.GamePlay.Abilities
{
    public class UpgradeSpecificFruitAbility : IAbility
    {
        [Inject]
        private SignalBus _signalBus;
        
        [Inject] 
        private IEffectsManager _effectsManager;
        
        [Inject]
        private InputDriven _inputDriven;
        
        [Inject]
        private IPlayerInput _playerInput;
        
        [Inject]
        private ItemFactory _itemFactory;
        
        public string Id => _abilityDataSo.Id;
        public int Count
        {
            get => _abilityDataSo.Count;
            set => _abilityDataSo.Count = value;
        }
        
        private AbilityDataSO _abilityDataSo;
        private bool _isWaitingForClick = false;

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

            if (IsJarEmpty())
                return;
            
            Count--;
            _signalBus.Fire<DisableUI>();
            
            _inputDriven.BlockInput();

            _isWaitingForClick = true;
            await WaitForUserClick();
            
            await UniTask.Delay(TimeSpan.FromSeconds(0.5));
            _inputDriven.UnblockInput();
            
            await UniTask.Delay(TimeSpan.FromSeconds(0.5));
            _signalBus.Fire<EnableUI>();
            
        }

        private bool IsJarEmpty()
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

        private bool IsOutsideTheJar(Item item)
        {
            return item.gameObject.transform.position.y >= 2.5;
        }

        private async UniTask  WaitForUserClick()
        {
            while (_isWaitingForClick)
            {
                if (_playerInput.IsClickRequested)
                {
                    Vector2 worldPosition = _playerInput.GetClickPosition;
                    
                    Collider2D hit = Physics2D.OverlapPoint(worldPosition);

                    if (hit != null)
                    {
                        Item clickedItem = hit.GetComponent<Item>();

                        if (clickedItem != null)
                        {
                            HandleUpgrade(clickedItem);
                            _isWaitingForClick = false;
                        }
                    }
                }
                
                await UniTask.Yield();
            }
        }

        private void HandleUpgrade(Item clickedItem)
        {
            int currentLevel = clickedItem.GetItemId();
            Vector2 currItemPos = clickedItem.transform.position;
            
            Object.Destroy(clickedItem.gameObject);
            
            int upgradedLevel = currentLevel + 1;

            if (upgradedLevel >= 11)
            {
                _effectsManager.PlayEffect(EffectType.Destroy, currItemPos);
                return;
            }
            
            var newItem = _itemFactory.Create(upgradedLevel);
            _effectsManager.PlayEffect(EffectType.Destroy, currItemPos);
            
            if (newItem != null)
            {
                newItem.GetComponent<Rigidbody2D>().gravityScale = 0;
                newItem.transform.position = currItemPos;
            }
            else
            {
                Debug.LogError("Failed to create upgraded item!");
            }
        }
    }
}