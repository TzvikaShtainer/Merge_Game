using UnityEngine;
using VisualLayer.GamePlay.PlayerInput;
using Zenject;

namespace VisualLayer.Components.UI
{
    public class InputDriven : MonoBehaviour
    {
        #region MyRegion
        
        [Inject]
        private IPlayerInput _playerInput;
        

        #endregion

        private void Update()
        {
            if (_playerInput == null)
            {
                return;
            }

            if (_playerInput.IsClickRequested)
            {
                Fire();
            }
        }

        private void Fire()
        {
            //Debug.Log("Fire");
            return;
        }
    }
}