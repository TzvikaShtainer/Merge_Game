using Cysharp.Threading.Tasks;

namespace VisualLayer.GamePlay.Popups.MusicMenuPopup
{
    public interface ISettingsMenuActions
    {
        void OnToggleMusic();
        void OnToggleSfx();
        void OnToggleVibration();
        UniTask OnRestartGame();
    }
}