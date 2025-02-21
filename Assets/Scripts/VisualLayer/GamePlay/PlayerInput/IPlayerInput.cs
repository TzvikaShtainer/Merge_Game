namespace VisualLayer.GamePlay.PlayerInput
{
    public interface IPlayerInput
    {
        float GetHorizontalInput{ get; }
        bool IsClickRequested { get; }
    }
}