using System;
using EasyUI.PickerWheelUI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using VisualLayer.GamePlay.Handlers.StartScene;
using Zenject;

namespace VisualLayer.GamePlay.Popups.SpinTheWheelPopup
{
    public class SpinTheWheelPopup : Popup
    {
        [Inject]
        private IWheelHandler  _wheelHandler;
        
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
        public PickerWheel PickerWheel => _pickerWheel;
        
        #endregion
        
        #region Methods
        public void OnCloseBtnClick() => Close();

        public void OnSpinBtnClick()
        {
            _wheelHandler.SpinWheel();
        }

        private void Start()
        {
            _wheelHandler.SetWheel(_pickerWheel);
        }

        private new void OnEnable()
        {
            base.OnEnable();
            
            _wheelHandler.OnSpinStarted += OnSpinStart;
            _wheelHandler.OnSpinEnded += OnSpinEnd;
        }

        private void OnDisable()
        {
            _wheelHandler.OnSpinStarted -= OnSpinStart;
            _wheelHandler.OnSpinEnded -= OnSpinEnd;
        }

        private void OnSpinStart()
        {
            _spinTheWheelButton.interactable = false;
            _spinTheWheelText.text = "Spinning";
        }
        
        private void OnSpinEnd(WheelPiece wheelPiece)
        {
            _spinTheWheelButton.interactable = true;
            _spinTheWheelText.text = "Spin";
        }

        #endregion
    }
}