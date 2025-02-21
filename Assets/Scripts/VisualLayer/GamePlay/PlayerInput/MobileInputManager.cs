using UnityEngine;
using VisualLayer.Components.UI.Joystick;

namespace VisualLayer.GamePlay.PlayerInput
{
    public class MobileInputManager : IPlayerInput
    {
        #region Editor

        [SerializeField]
        private Joystick _joystick;
		
        #endregion


        public float GetHorizontalInput => Input.mousePosition.y; //just for now

        public bool IsClickRequested => Input.GetMouseButtonDown(0); //just for now
    }
}