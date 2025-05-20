
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
    public class DestroySpecificFruitAbility : BaseAbility
    {
        [Inject] 
        private IEffectsManager _effectsManager;
        
        [Inject]
        private InputDriven _inputDriven;
        
        [Inject]
        private IPlayerInput _playerInput;
        
        [Inject]
        private ItemFactory _itemFactory;
        
        public AbilityDataSO Data => _abilityDataSo;
        
        private AbilityDataSO _abilityDataSo;
        private bool _isWaitingForClick = false;
        
        public override async void UseAbility()
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
                            DestroySpecificFruit(clickedItem);
                            _isWaitingForClick = false;
                            FMODUnity.RuntimeManager.PlayOneShot(FModEvents.Instance.RemovePowerup);
                        }
                    }
                }
                
                await UniTask.Yield();
            }
        }

        private void DestroySpecificFruit(Item clickedItem)
        {
            _effectsManager.PlayEffect(EffectType.DestroyAbility, clickedItem.transform.position);
            Object.Destroy(clickedItem.gameObject);
        }
        
        public override void EnableEnvironment()
        {
            SignalBus.Fire<EnableUI>();
            _inputDriven.UnblockInput();
            
            EnableItemsOutsideTheJar();
        }
    }
}