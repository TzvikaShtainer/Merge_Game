using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DataLayer;
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
    public class UpgradeSpecificFruitAbility : BaseAbility
    {
        [Inject] 
        private IEffectsManager _effectsManager;
        
        [Inject]
        private InputDriven _inputDriven;
        
        [Inject]
        private IPlayerInput _playerInput;
        
        [Inject]
        private ItemFactory _itemFactory;
        
        private bool _isWaitingForClick = false;
        
        public override  async void UseAbility()
        {
            if (Count <= 0)
                return;

            if (IsJarEmpty())
                return;
            
            Count--;
            
            
            DisableEnvironment(); 

            _isWaitingForClick = true;
            await WaitForUserClick();
            
            await UniTask.Delay(TimeSpan.FromSeconds(0.5));
            
            EnableEnvironment();
            
        }
        
        protected override void DisableEnvironment()
        {
            base.DisableEnvironment();
            
            _inputDriven.BlockInput();
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
                //newItem.GetComponent<Rigidbody2D>().gravityScale = 0;
                newItem.transform.position = currItemPos;
            }
            else
            {
                Debug.LogError("Failed to create upgraded item!");
            }
        }
        
        public override void EnableEnvironment()
        {
            SignalBus.Fire<EnableUI>();
            _inputDriven.UnblockInput();
            
            EnableItemsOutsideTheJar();
        }
    }
}