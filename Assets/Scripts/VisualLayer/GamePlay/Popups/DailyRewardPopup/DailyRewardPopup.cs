using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace VisualLayer.GamePlay.Popups.DailyRewardPopup
{
    public class DailyRewardPopup : Popup
    {
        #region Injects

        [Inject] 
        private IDailyRewardCooldownService _dailyRewardService;

        #endregion
        
        #region Factory

        public class Factory : PlaceholderFactory<DailyRewardPopup> {}

        #endregion

        #region Editor

        [SerializeField] private Button claimButton;
        [SerializeField] private TextMeshProUGUI timerText;
        [SerializeField] private TextMeshProUGUI dayText;

        #endregion

        #region Methods

        private void Update()
        {
            if (_dailyRewardService.CanClaim(out var remaining))
            {
                claimButton.interactable = true;
                timerText.text = "Claim Reward!";
            }
            else
            {
                claimButton.interactable = false;
                timerText.text = $"{remaining.Hours:D2}:{remaining.Minutes:D2}:{remaining.Seconds:D2}";
            }

            dayText.text = $"Day {_dailyRewardService.CurrentDayIndex()}/7";
        }
        
        public void OnClaimButtonClick()
        {
            _dailyRewardService.Claim();
        }
        
        public void OnCloseBtnClick()
        {
            Close();
        }

        #endregion
    }
}