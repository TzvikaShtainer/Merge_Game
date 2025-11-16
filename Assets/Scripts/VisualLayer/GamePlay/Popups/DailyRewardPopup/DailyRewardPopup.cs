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
        [SerializeField] private Transform rewardsHidersTransforms;

        #endregion

        #region Methods

        protected override void OnEnable()
        {
            base.OnEnable();
            
            RefreshStaticUI();
            UpdateTimerAndButton();
        }
        
        private void Update()
        {
            UpdateTimerAndButton();
        }
        
        private void RefreshStaticUI()
        {
            SetRewardsHidersTransforms();
        }

        private void UpdateTimerAndButton()
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

        private void SetRewardsHidersTransforms()
        {
            int totalSlots = rewardsHidersTransforms.childCount;
            int currentDayIndex = _dailyRewardService.CurrentDayIndex();
            int claimedCount = Mathf.Clamp(currentDayIndex, 0, totalSlots);

            for (int i = 0; i < totalSlots; i++)
            {
                bool isClaimed = i < claimedCount;
                var hider = rewardsHidersTransforms.GetChild(i).gameObject;
                hider.SetActive(!isClaimed);
            }
        }

        public void OnClaimButtonClick()
        {
            _dailyRewardService.Claim();
            SetRewardsHidersTransforms();
        }
        
        public void OnCloseBtnClick()
        {
            Close();
        }

        #endregion
    }
}