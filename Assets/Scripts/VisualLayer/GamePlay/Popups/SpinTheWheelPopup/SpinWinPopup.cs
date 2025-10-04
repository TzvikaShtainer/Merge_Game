using EasyUI.PickerWheelUI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace VisualLayer.GamePlay.Popups.SpinTheWheelPopup
{
    public class SpinWinPopup : Popup
    {
        #region Factories
        public class Factory : PlaceholderFactory<Sprite, int, SpinWinPopup>
        {
            
        }
        #endregion
        
        #region Editor
        [SerializeField] 
        private Image _winItetemSprite;
        
        [SerializeField]
        private TextMeshProUGUI  _winitemAmount;
        
        #endregion

        #region Methods

        [Inject]
        public void Construct(Sprite winItemSprite, int winItemAmount)
        {
            _winItetemSprite.sprite = winItemSprite;
            _winitemAmount.text = $"Amount: {winItemAmount.ToString()}";
        }
        public void OnCloseBtnClick() => Close();

        #endregion
        
        
        
    }
}