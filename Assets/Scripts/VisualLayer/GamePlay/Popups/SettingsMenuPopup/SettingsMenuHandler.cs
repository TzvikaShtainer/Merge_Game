using UnityEngine;

namespace VisualLayer.GamePlay.Popups.MusicMenuPopup
{
    public class SettingsMenuHandler : ISettingsMenuActions
    {
        public void OnToggleMusic()
        {
           Debug.Log("OnToggleMusic");
        }

        public void OnToggleSfx()
        {
            Debug.Log("OnToggleSfx");
        }

        public void OnToggleVibration()
        {
            Debug.Log("OnToggleVibration");
        }

        public void OnRestartGame()
        {
            Debug.Log("OnRestartGame");
        }

        public void OnCloseMenu()
        {
            Debug.Log("OnCloseMenu");
        }
    }
}