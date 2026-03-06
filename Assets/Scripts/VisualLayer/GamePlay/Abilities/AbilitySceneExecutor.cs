using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DataLayer.DataTypes;
using DataLayer.DataTypes.abilities;
using DG.Tweening;
using ServiceLayer.EffectsService;
using ServiceLayer.MusicService;
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
    public class AbilitySceneExecutor : MonoBehaviour
    {
        [Inject] 
        private List<IAbility> _abilities;
        
        [Inject]
        private  SignalBus _signalBus;
        
        [Inject]
        private ISfxService  _sfxService;
        
        [Inject] 
        private IEffectsManager _effectsManager;
        
        [Inject]
        private InputDriven _inputDriven;
        
        [Inject]
        private IPlayerInput _playerInput;
        
        [Inject]
        private ItemFactory _itemFactory;

        [Inject] 
        private AbilityManager _abilityManager;
        
        //General 
        private List<Item> _itemsToToggle;
        private List<Item> _allItems;
        
        //ShakeBox Ability
        [Inject(Id = "ShakeBoxJar")]
        private Transform _jarTransform;
        
        [Inject(Id = "MainGameplayCamera")]
        private Camera _mainCamera;
        
        private float _originalOrthoSize;
        
        //UpgradeSpecificFruitAbility && DestroySpecificFruitAbility
        private bool _isWaitingForClick = false;
        
        //DestroySpecificFruitAbility
        private AbilityDataSO _abilityDataSo;
        
        private void OnEnable()
        {
            foreach (var ability in _abilities)
            {
                if (ability is BaseAbility baseAbility)
                    baseAbility.OnRequestExecution += () => HandleExecution(baseAbility);
            }
        }

        private void OnDisable()
        {
            foreach (var ability in _abilities)
            {
                if (ability is BaseAbility ba)
                    ba.OnRequestExecution -= () => HandleExecution(ba); //change this
            }
        }
        
        private async void HandleExecution(BaseAbility ability)
        {
            if (IsJarEmpty()) return;

            DisableEnvironment();
            
            bool executionSuccess = false;

            try
            {
                switch (ability.Id)
                {
                    case "ShakeBoxAbility":
                        await ExecuteShakeBoxAbility();
                        executionSuccess = true;
                        break;

                    case "UpgradeSpecificFruitAbility":
                        await ExecuteUpgradeSpecificFruitAbility();
                        executionSuccess = true;
                        break;

                    case "DestroyAllLowestLevelFruitsAbility":
                        ExecuteDestroyAllLowestLevelFruitsAbility();
                        executionSuccess = true;
                        break;

                    case "DestroySpecificFruitAbility":
                        await ExecuteDestroySpecificFruitAbility();
                        executionSuccess = true;
                        break;

                    case "DestroyItemsAfterContinue":
                        ExecuteDestroyItemsAfterContinue();
                        executionSuccess = true;
                        break;
                }
                
                if (executionSuccess)
                {
                    _abilityManager.ConsumeAbility(ability.Id);
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            finally 
            {
                EnableItemsOutsideTheJar();
            }
        }

        #region Shared Methods

        protected virtual void DisableEnvironment()
        {
            _itemsToToggle = new List<Item>();
            
            _allItems = Object.FindObjectsOfType<Item>().ToList();
            
            Debug.Log($"<color=white>[AbilityExecutor] Scanning {_allItems.Count} total items in scene...</color>");
            
            _signalBus.Fire<DisableUISignal>();
            
            SortItems();
            
            string itemList = string.Join(", ", _itemsToToggle.Select(i => $"{i.name} (Active: {i.gameObject.activeSelf}, Y: {i.transform.position.y:F2})"));
            Debug.Log($"<color=orange>[AbilityExecutor] Found {_itemsToToggle.Count} items to toggle: {itemList}</color>");
            
            DisableItemsOutsideTheJar();
        }
        
        private void SortItems()
        {
            foreach (Item currItem in _allItems)
            {
                if (IsOutsideTheJar(currItem))
                {
                    _itemsToToggle.Add(currItem);
                }
            }
        }

        protected bool IsOutsideTheJar(Item currItem)
        {
            return currItem.transform.position.y >= 2.5f;
        }
        
        private void DisableItemsOutsideTheJar()
        {
            ToggleItems(false);
        }
        
        private void ToggleItems(bool isEnabled)
        {
            Debug.Log("before if");
            if (_itemsToToggle == null || _itemsToToggle.Count == 0)
            {
                Debug.LogWarning($"<color=red>[AbilityExecutor] ToggleItems({isEnabled}) called but _itemsToToggle is EMPTY!</color>");
                return;
            }

            foreach (Item currItem in _itemsToToggle)
            {
                if (currItem != null)
                {
                    Debug.Log($"[AbilityExecutor] Setting {currItem.name} Active = {isEnabled}");
                    currItem.gameObject.SetActive(isEnabled);
                }
                else
                {
                    Debug.LogError("[AbilityExecutor] A null item was found in the toggle list during execution!");
                }
            }
        }

        protected void EnableItemsOutsideTheJar()
        {
            Debug.Log($"<color=green>[AbilityExecutor] Enabling items back. Count: {_itemsToToggle?.Count ?? 0}</color>");
            ToggleItems(true);
        }
        
        protected bool IsJarEmpty()
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

        #endregion
        
        #region ShakeBoxAbility

        private async UniTask ExecuteShakeBoxAbility()
        {
            _signalBus.Fire<PauseInputSignal>();
            _signalBus.Fire<DisableLoseCollider>();
            
            ZoomOutFOV();
            await UniTask.Delay(TimeSpan.FromSeconds(0.5));
            
            ShakeBox();
            await UniTask.Delay(TimeSpan.FromSeconds(2.5));
            
            ZoomInFOV();
            await UniTask.Delay(TimeSpan.FromSeconds(0.5));

            
            await UniTask.Delay(TimeSpan.FromSeconds(0.5));
            
            
            _signalBus.Fire<UnpauseInputSignal>();
            _signalBus.Fire<EnableUISignal>();
            _signalBus.Fire<EnableLoseCollider>();
            
            EnableItemsOutsideTheJar();
        }
        private void ShakeBox()
        {
            //cancel previous shake
            _jarTransform.DOKill();

            //save start location
            Vector3 startRotation = _jarTransform.eulerAngles;
            float shakeAngle = 15f; 
            float shakeDuration = 0.3f;

            _sfxService.PlaySfxType(SfxType.ShakeBoxAbility);
            
            Sequence shakeSequence = DOTween.Sequence();
            
            //Right shake
            shakeSequence.Append(
                _jarTransform.DORotate(
                    new Vector3(startRotation.x, startRotation.y, startRotation.z + shakeAngle),
                    shakeDuration
                ).SetEase(Ease.InOutSine).SetUpdate(UpdateType.Fixed)
            );
            
            //Middle back
            shakeSequence.Append(
                _jarTransform.DORotate(
                    new Vector3(startRotation.x, startRotation.y, startRotation.z),
                    shakeDuration
                ).SetEase(Ease.InOutSine).SetUpdate(UpdateType.Fixed)
            );
             
            //Left shake
            shakeSequence.Append(
                _jarTransform.DORotate(
                    new Vector3(startRotation.x, startRotation.y, startRotation.z - shakeAngle),
                    shakeDuration
                ).SetEase(Ease.InOutSine).SetUpdate(UpdateType.Fixed)
            );

            //Middle back
            shakeSequence.Append(
                _jarTransform.DORotate(
                    new Vector3(startRotation.x, startRotation.y, startRotation.z),
                    shakeDuration
                ).SetEase(Ease.InOutSine).SetUpdate(UpdateType.Fixed)
            );

            //How Many Times
            shakeSequence.SetLoops(2);

            //Back To Middle
            shakeSequence.OnComplete(() =>
            {
                _jarTransform.rotation = Quaternion.Euler(startRotation);
            });
        }
        
        private void ZoomOutFOV()
        {
            if (_mainCamera == null)
            {
                //Debug.LogWarning("Main camera not assigned!");
                return;
            }

            _mainCamera.DOKill();

            if (_mainCamera.orthographic)
            {
                // אם המצלמה אורטוגרפית, נשמור את ה-orthographicSize
                _originalOrthoSize = _mainCamera.orthographicSize;

                float targetOrthoSize = _originalOrthoSize + 2f; // כמה להתרחב
                float zoomDuration = 0.5f;

                DOTween.To(
                    () => _mainCamera.orthographicSize,
                    x => _mainCamera.orthographicSize = x,
                    targetOrthoSize,
                    zoomDuration
                ).SetEase(Ease.InOutSine);
            }
            else
            {
                //Debug.LogWarning("Camera is not orthographic, expected orthographic mode.");
            }
        }

        private void ZoomInFOV()
        {
            if (_mainCamera == null)
            {
               // Debug.LogWarning("Main camera not assigned!");
                return;
            }

            _mainCamera.DOKill();

            if (_mainCamera.orthographic)
            {
                float zoomDuration = 0.5f;

                DOTween.To(
                    () => _mainCamera.orthographicSize,
                    x => _mainCamera.orthographicSize = x,
                    _originalOrthoSize,
                    zoomDuration
                ).SetEase(Ease.InOutSine);
            }
            else
            {
                //Debug.LogWarning("Camera is not orthographic, expected orthographic mode.");
            }
        }

        #endregion

        #region UpgradeSpecificFruitAbility

        private async UniTask ExecuteUpgradeSpecificFruitAbility()
        {
            _inputDriven.BlockInput();
            
            _isWaitingForClick = true;
            
            await WaitForUserClick();
            
            await UniTask.Delay(TimeSpan.FromSeconds(0.5));
            
            _signalBus.Fire<EnableUISignal>();
            _inputDriven.UnblockInput();
            
            EnableItemsOutsideTheJar();
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
            
            _sfxService.PlaySfxType(SfxType.UpgradeSpecificItemAbility);
            
            int upgradedLevel = currentLevel + 1;

            if (upgradedLevel >= 11)
            {
                _effectsManager.PlayEffect(EffectType.DestroyAbility, currItemPos);
                _sfxService.PlaySfxType(SfxType.DestroyAbility);
                return;
            }
            
            var newItem = _itemFactory.Create(upgradedLevel,currItemPos);
            newItem.MakeItemFall(true);
            
            _effectsManager.PlayEffect(EffectType.DestroyAbility, currItemPos);
            _sfxService.PlaySfxType(SfxType.DestroyAbility);
            
            // if (newItem != null)
            // {
            //     //newItem.GetComponent<Rigidbody2D>().gravityScale = 0;
            //     //newItem.transform.position = currItemPos;
            // }
            // else
            // {
            //     Debug.LogError("Failed to create upgraded item!");
            // }
        }

        #endregion

        #region DestroyAllLowestLevelFruitsAbility

        private async UniTask ExecuteDestroyAllLowestLevelFruitsAbility()
        {
            _signalBus.Fire<PauseInputSignal>();
            
            FindAndDestroyLowestItems();    
            
            await UniTask.Delay(TimeSpan.FromSeconds(0.5));
            
            _signalBus.Fire<UnpauseInputSignal>();
            _signalBus.Fire<EnableUISignal>();
            
            EnableItemsOutsideTheJar();
        }
        
        private void FindAndDestroyLowestItems()
        {
            List<Item> allItems = Object.FindObjectsOfType<Item>().ToList();
            
            allItems = RemoveItemsThatNotInTheJar(allItems);

            if (allItems.Count == 0)
            {
                //Debug.Log("No items found");
                return;
            }

            int lowestItemIndex = allItems.Min(item => item.GetItemId());

            DestroyLowestItems(allItems, lowestItemIndex);
        }

        private void DestroyLowestItems(List<Item> allItems, int lowestItemIndex)
        {
            for (var index = 0; index < allItems.Count; index++)
            {
                var currItem = allItems[index];
                if (currItem.GetItemId() == lowestItemIndex && !IsOutsideTheJar(currItem))
                {
                    Object.Destroy(currItem.gameObject);
                    _effectsManager.PlayEffect(EffectType.DestroyAbility, currItem.gameObject.transform.position);
                    _sfxService.PlaySfxType(SfxType.DestroyAbility);
                }
            }
        }

        private List<Item> RemoveItemsThatNotInTheJar(List<Item> allItems)
        {
            allItems = allItems
                .Where(item => !IsOutsideTheJar(item))
                .ToList();
            return allItems;
        }

        #endregion

        #region DestroySpecificFruitAbility

        private async UniTask ExecuteDestroySpecificFruitAbility()
        {
            _inputDriven.BlockInput();
            
            _isWaitingForClick = true;
            await WaitForUserClickDestroy();
            
            await UniTask.Delay(TimeSpan.FromSeconds(0.5));
            
            _signalBus.Fire<EnableUISignal>();
            _inputDriven.UnblockInput();
            
            EnableItemsOutsideTheJar();
        }

        private async UniTask  WaitForUserClickDestroy()
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
                        }
                    }
                }
                
                await UniTask.Yield();
            }
        }

        private void DestroySpecificFruit(Item clickedItem)
        {
            _effectsManager.PlayEffect(EffectType.DestroyAbility, clickedItem.transform.position);
            _sfxService.PlaySfxType(SfxType.DestroyAbility);
            Object.Destroy(clickedItem.gameObject);
        }
        
        #endregion

        #region DestroyItemsAfterContinue

        private async UniTask ExecuteDestroyItemsAfterContinue()
        {
            _signalBus.Fire<PauseInputSignal>();
            
            FindAndDestroyTwoLowestItems();    
            
            await UniTask.Delay(TimeSpan.FromSeconds(1.5));
            
            _signalBus.Fire<UnpauseInputSignal>();
            _signalBus.Fire<EnableUISignal>();
            
            EnableItemsOutsideTheJar();
        }

        private void FindAndDestroyTwoLowestItems()
        {
            List<Item> itemsInJar = Object.FindObjectsOfType<Item>()
                .Where(item => !IsOutsideTheJar(item))
                .ToList();

            if (itemsInJar.Count == 0) return;

            var distinctIds = itemsInJar
                .Select(item => item.GetItemId())
                .Distinct()
                .OrderBy(id => id)
                .Take(2) 
                .ToList();

            if (distinctIds.Count == 0) return;

            foreach (int idToDestroy in distinctIds)
            {
                var targets = itemsInJar.Where(i => i.GetItemId() == idToDestroy).ToList();
        
                foreach (var target in targets)
                {
                    ExecuteDestruction(target);
                }
            }
        }

        private void ExecuteDestruction(Item item)
        {
            if (item == null) return;
    
            _effectsManager.PlayEffect(EffectType.DestroyAbility, item.transform.position);
            _sfxService.PlaySfxType(SfxType.DestroyAbility);
            Object.Destroy(item.gameObject);
        }
        
        #endregion
    }
}