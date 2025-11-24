using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VisualLayer.GamePlay.RewardSystem;

namespace VisualLayer.GamePlay.UI
{
    public class DailyRewardItemUI : MonoBehaviour
    {
        [SerializeField] private Image rewardIcon;
        [SerializeField] private TextMeshProUGUI rewardAmountText;

        public void Init(DailyRewardsConfig.Reward reward)
        {
            if (rewardIcon != null)
            {
                rewardIcon.sprite = reward.RewardIcon;
                rewardIcon.enabled = reward.RewardIcon != null;
            }

            if (rewardAmountText != null)
            {
                rewardAmountText.text = reward.RewardAmount.ToString();
            }
        }
    }
}