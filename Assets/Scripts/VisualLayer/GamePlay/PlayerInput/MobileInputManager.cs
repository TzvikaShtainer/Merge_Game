using System;
using UnityEngine;
using VisualLayer.Components.UI.Joystick;

namespace VisualLayer.GamePlay.PlayerInput
{
    public class MobileInputManager : MonoBehaviour, IPlayerInput
    {
        #region Editor

        [SerializeField]
        private Joystick _joystick;
		
        #endregion


        public float GetHorizontalInput => Camera.main.ScreenToWorldPoint(Input.mousePosition).x;

        public bool IsClickRequested => _joystick.IsPressed;
        public Vector2 GetClickPosition  => Camera.main.ScreenToWorldPoint(Input.mousePosition);

        public event Action OnRelease
        {
            add { _joystick.OnReleased += value; }
            remove { _joystick.OnReleased -= value; }
        }
    }
}