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
        [SerializeField] private TextMeshProUGUI _hourlyCoinsButtonText;

        private void Update()
        {
            if (_coinsService.CanClaim(out TimeSpan remaining))
            {
                _hourlyCoinsButton.interactable = true;
                _hourlyCoinsButtonText.text = $"{_coinsService.GetRewardAmount().ToString()}";
            }
            else
            {
                _hourlyCoinsButton.interactable = false;
                _hourlyCoinsButtonText.text = $"{remaining.Minutes:D2}:{remaining.Seconds:D2}";
            }
        }

        public void OnClick()
        {
            base.OnClick();
            _hourlyCoinsClickHandler.Execute();
        }
    }
}