using DataLayer.DataTypes;
using ServiceLayer.EffectsService;
using ServiceLayer.Signals.SignalsClasses;
using ServiceLayer.Utilis;
using UnityEngine;
using VisualLayer.GamePlay.PlayerInput;
using VisualLayer.MergeItems.SpawnLogic;
using Zenject;

namespace VisualLayer.Components.UI
{
    public class InputDriven : MonoBehaviour
    {
        #region Injects
        
        [Inject]
        private IPlayerInput _playerInput;
        
        [Inject]
        private ISpawn _spawn;
        
        [Inject] 
        private IEffectsManager _effectsManager;
        
        private DelayTimer _delayTimer;
        private bool _waitingForNextItem = false;
        private Vector2 _lastClickedPos;
        private readonly float _delayTime = 0.5f;
        private bool _isInputBlocked = false;
        
        #endregion

        private void Start()
        {
            _delayTimer = new DelayTimer(_delayTime);
            
            if (_playerInput is MobileInputManager mobileInput)
            {
                mobileInput.OnRelease += OnRelease;
            }
        }
        
        private void Update()
        {
            if (_isInputBlocked) 
                return;
            
            if (_waitingForNextItem && _delayTimer.IsReady)
            {
                CompleteSpawn();
            }

            if (!_delayTimer.IsReady || _waitingForNextItem)
                return;

            if (_playerInput.IsClickRequested)
            {
                Vector2 newPos = new Vector2(_playerInput.GetHorizontalInput, 2.5f);
                _spawn.UpdateDraggingPosition(newPos);
            }
        }
        
        private void CompleteSpawn()
        {
            _spawn.CompleteSpawn(_lastClickedPos); 
            _waitingForNextItem = false;
        }
        
        private void OnRelease()
        {
            if (_isInputBlocked)
                return;
            
            if (!_delayTimer.IsReady)
                return;

            _lastClickedPos = new Vector2(_playerInput.GetHorizontalInput, 2.5f);
            _spawn.Spawn(_lastClickedPos); 
            
            _effectsManager.PlayEffect(EffectType.Release, _playerInput.GetClickPosition); //_lastClickedPos just for now, need to change
            
            _waitingForNextItem = true;
            _delayTimer.Reset();
        }
        
        private void OnDestroy()
        {
            if (_playerInput is MobileInputManager mobileInput)
            {
                mobileInput.OnRelease -= OnRelease;
            }
        }

        public void BlockInput()
        {
            _isInputBlocked = true;
        }

        public void UnblockInput()
        {
            _isInputBlocked = false;
        }
        
    }
}