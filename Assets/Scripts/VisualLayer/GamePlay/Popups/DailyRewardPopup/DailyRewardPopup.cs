using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VisualLayer.GamePlay.RewardSystem;
using VisualLayer.GamePlay.UI;
using Zenject;

namespace VisualLayer.GamePlay.Popups.DailyRewardPopup
{
    public class DailyRewardPopup : Popup
    {
        #region Injects

        [Inject] 
        private IDailyRewardCooldownService _dailyRewardService;
        
        [Inject]
        private DailyRewardsConfig _dailyRewardsConfig;

        #endregion
        
        #region Factory

        public class Factory : PlaceholderFactory<DailyRewardPopup> {}

        #endregion

        #region Editor
        
        [System.Serializable]
        public class DailyRewardDayUI
        {
            public Transform rewardsContainer;
            public GameObject hider;
        }
        

        [SerializeField] private Button claimButton;
        [SerializeField] private TextMeshProUGUI timerText;
        [SerializeField] private TextMeshProUGUI dayText;
        
        [SerializeField] private DailyRewardDayUI[] daysUI;
        [SerializeField] private DailyRewardItemUI rewardItemPrefab;

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
            SetupRewardsVisuals();
            UpdateHiders();
        }

        private void SetupRewardsVisuals()
        {
            if (rewardItemPrefab == null || _dailyRewardsConfig == null || daysUI == null || daysUI.Length == 0)
                return;

            int totalDays = Mathf.Min(_dailyRewardsConfig.TotalDays, daysUI.Length);

            for (int day = 1; day <= totalDays; day++)
            {
                var dayUI = daysUI[day - 1];
                var configDay = _dailyRewardsConfig.GetRewardForDay(day);

                if (dayUI.rewardsContainer == null)
                    continue;
                
                //clean childs
                for (int i = dayUI.rewardsContainer.childCount - 1; i >= 0; i--)
                {
                    Destroy(dayUI.rewardsContainer.GetChild(i).gameObject);
                }

                if (configDay?.DailyRewards == null || configDay.DailyRewards.Count == 0)
                    continue;

                foreach (var reward in configDay.DailyRewards)
                {
                    var rewardItem = Instantiate(rewardItemPrefab, dayUI.rewardsContainer);
                    rewardItem.Init(reward);
                }
            }
        }

        private void UpdateTimerAndButton()
        {
            if (_dailyRewardService.CanClaim(out var remaining))
            {
                claimButton.interactable = true;
                timerText.text = "Tap to Collect";
            }
            else
            {
                claimButton.interactable = false;
                timerText.text = "";
                //timerText.text = $"{remaining.Hours:D2}:{remaining.Minutes:D2}:{remaining.Seconds:D2}";
            }

            dayText.text = "";
            //dayText.text = $"Day {_dailyRewardService.CurrentDayIndex()}/7";
        }

        private void UpdateHiders()
        {
            int currentDayIndex = _dailyRewardService.CurrentDayIndex(); 

            for (int i = 0; i < daysUI.Length; i++)
            {
                var dayUI = daysUI[i];
                if (dayUI.hider == null)
                    continue;
                
                bool isClaimed = i < currentDayIndex; // i=0 => Day 1

                dayUI.hider.SetActive(!isClaimed);
            }
        }

        public void OnClaimButtonClick()
        {
            _dailyRewardService.Claim();
            UpdateTimerAndButton();
            UpdateHiders();
        }
        
        public void OnCloseBtnClick()
        {
            Close();
        }

        #endregion
    }
}