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
        

        #endregion
        

        private void Start()
        {
            if (_playerInput is MobileInputManager mobileInput)
            {
                mobileInput.OnRelease += Spawn;
            }
        }
        
        private void Update()
        {
            if (_playerInput.IsClickRequested)
            {
                
            }
        }
        
        private void OnDestroy()
        {
            if (_playerInput is MobileInputManager mobileInput)
            {
                mobileInput.OnRelease -= Spawn;
            }
        }

        private void Spawn()
        {
            _spawn.Spawn(new Vector2(_playerInput.GetHorizontalInput, 2));
        }
    }
}