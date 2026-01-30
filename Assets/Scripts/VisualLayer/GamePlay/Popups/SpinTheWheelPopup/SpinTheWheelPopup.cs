using System;
using Cysharp.Threading.Tasks;
using EasyUI.PickerWheelUI;
using ServiceLayer.SpinTheWheelCooldownService;
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
        
        [Inject]
        private SpinWinPopup.Factory _spinWinPopupFactory;
        
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
        
        private bool _isSpinning;
        
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
            
            _wheelHandler.OnSpinStarted += OnSpinStarted;
            _wheelHandler.OnSpinEnded += OnSpinEnd;
        }

        private void OnDisable()
        {
            _wheelHandler.OnSpinStarted -= OnSpinStarted;
            _wheelHandler.OnSpinEnded -= OnSpinEnd;
        }

        private void OnSpinStarted()
        {
            _isSpinning = true;
            _spinTheWheelButton.interactable = false;
            _spinTheWheelText.text = "Spinning";
        }

        private async  void OnSpinEnd(WheelPiece wheelPiece)
        {
            _isSpinning = false;
            _spinWinPopupFactory.Create(wheelPiece.Icon, wheelPiece.Amount);
            await UniTask.Yield(); //for not spamming the spin btn
        }
        
        private void Update()
        {
            if (_isSpinning)
                return;
            
            if (_wheelHandler.CanSpin(out TimeSpan remaining))
            {
                _spinTheWheelButton.interactable = true;
                _spinTheWheelText.text = "Spin";
            }
            else
            {
                _spinTheWheelButton.interactable = false;
                _spinTheWheelText.text = $"{(int)remaining.TotalHours:D2}:{remaining.Minutes:D2}:{remaining.Seconds:D2}";
            }
        }

        #endregion
    }
}