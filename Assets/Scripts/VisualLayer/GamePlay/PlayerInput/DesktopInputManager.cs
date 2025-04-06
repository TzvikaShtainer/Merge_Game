using UnityEngine;

namespace VisualLayer.GamePlay.PlayerInput
{
    public class DesktopInputManager : IPlayerInput
    {
        public float GetHorizontalInput => Input.mousePosition.y;

        public bool IsClickRequested => Input.GetMouseButtonDown(0);
        public Vector2 GetClickPosition { get; }

        public bool IsTouchReleased //just copy for now
        {
            get 
            {
                if (Input.touchCount == 0) return false;
                foreach (var touch in Input.touches)
                {
                    if (touch.phase == TouchPhase.Ended)
                        return true;
                }
                return false;
            }
        }
    }
}