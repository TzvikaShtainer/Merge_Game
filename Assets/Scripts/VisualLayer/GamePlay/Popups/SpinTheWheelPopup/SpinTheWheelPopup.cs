using System;
using EasyUI.PickerWheelUI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VisualLayer.GamePlay.Handlers;
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
        [SerializeField] 
        private Button _spinTheWheelButton;
        
        [SerializeField]
        private TextMeshProUGUI  _spinTheWheelText;
        
        [SerializeField]
        private PickerWheel _pickerWheel;
        
        #endregion
        
        #region Methods
        public void OnCloseBtnClick() => Close();

        public void OnSpinBtnClick()
        {
            _pickerWheel.Spin();
        }

        private void Start()
        {
            _spinTheWheelButton.onClick.AddListener(() =>
            {
                _spinTheWheelButton.interactable = false;
                _spinTheWheelText.text = "Spinning";
                
                _pickerWheel.OnSpinEnd(wheelPiece =>
                {
                    _spinTheWheelButton.interactable = true;
                    _spinTheWheelText.text = "Spin";
                });
                
                
                
                _pickerWheel.Spin();
            });
        }

        #endregion
    }
}