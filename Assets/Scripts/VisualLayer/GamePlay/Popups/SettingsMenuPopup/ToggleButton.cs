using UnityEngine;

namespace VisualLayer.GamePlay.Popups.MusicMenuPopup
{
    [System.Serializable]
    public class ToggleButton
    {
        public UnityEngine.UI.Image buttonImage;
        public Sprite spriteOn;
        public Sprite spriteOff;
        public bool isOn = true;

        public void Toggle()
        {
            isOn = !isOn;
            buttonImage.sprite = isOn ? spriteOn : spriteOff;
        }
        
        //-----TODO: Save States------
    }
}