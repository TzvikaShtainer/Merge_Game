using System;
using ServiceLayer.HourlyCoinsService;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace VisualLayer.GamePlay.Buttons
{
    public class HourlyCoinsButton : UIButtonFeedback
    {
        [Inject]
        private IHourlyCoinsClickHandler _hourlyCoinsClickHandler;
        
        [Inject] 
        private IHourlyCoinsService _coinsService;
        
        [SerializeField] private Button _hourlyCoinsButton;
        [SerializeField] private TextMeshProUGUI _nextText;
        [SerializeField] private TextMeshProUGUI _CollectText;
        [SerializeField] private TextMeshProUGUI _hourlyCoinsButtonText;
        [SerializeField] private TextMeshProUGUI _timerText;
        [SerializeField] private Image _coinsImg;

        private void Update()
        {
            if (_coinsService.CanClaim(out TimeSpan remaining))
            {
                _hourlyCoinsButton.interactable = true;
                _nextText.text = "";
                _timerText.text = "";
                _CollectText.text = "Collect";
                _coinsImg.enabled = true;
                _hourlyCoinsButtonText.text = $"{_coinsService.GetRewardAmount().ToString()}";
            }
            else
            {
                _hourlyCoinsButton.interactable = false;
                _nextText.text = "Next reward in:";
                _CollectText.text = "";
                _coinsImg.enabled = false;
                _timerText.text = $"{remaining.Minutes:D2}:{remaining.Seconds:D2}";
                _hourlyCoinsButtonText.text = "";
            }
        }

        public void OnClick()
        {
            base.OnClick();
            _hourlyCoinsClickHandler.Execute();
        }
    }
}