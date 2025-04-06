using UnityEngine;

namespace VisualLayer.GamePlay.PlayerInput
{
    public interface IPlayerInput
    {
        float GetHorizontalInput{ get; }
        bool IsClickRequested { get; }
        
        Vector2 GetClickPosition { get; }
        
    }
}