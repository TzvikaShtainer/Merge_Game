using UnityEngine;

namespace VisualLayer.GamePlay.PlayerInput
{
    public class DesktopInputManager : IPlayerInput
    {
        public float GetHorizontalInput => Input.mousePosition.y;

        public bool IsClickRequested => Input.GetMouseButtonDown(0);
    }
}