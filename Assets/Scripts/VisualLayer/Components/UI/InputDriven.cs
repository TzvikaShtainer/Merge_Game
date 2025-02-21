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
        

        private void Update()
        {
            if (_playerInput == null)
            {
                return;
            }

            if (_playerInput.IsClickRequested)
            {
                Spawn();
            }
        }

        private void Spawn()
        {
            _spawn.Spawn();
        }
    }
}