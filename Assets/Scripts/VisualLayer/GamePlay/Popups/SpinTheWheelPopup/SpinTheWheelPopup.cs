using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace VisualLayer.GamePlay.Popups.SpinTheWheelPopup
{
    public class SpinTheWheelPopup : Popup
    {
        #region Factories
        public class Factory : PlaceholderFactory<SpinTheWheelPopup>
        {
            
        }
        #endregion
        
        #region Editor
        [SerializeField] Button _spinTheWheelButton;
        
        #endregion
        
        #region Methods
        public void OnCloseBtnClick() => Close();
        #endregion
    }
}