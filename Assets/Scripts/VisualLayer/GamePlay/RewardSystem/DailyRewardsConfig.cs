using System.Collections.Generic;
using UnityEngine;

namespace VisualLayer.GamePlay.RewardSystem
{
    [CreateAssetMenu(menuName = "Game/Daily Rewards Config", fileName = "DailyRewardsConfig")]
    public class DailyRewardsConfig: ScriptableObject
    {
        [System.Serializable]
        public class DailyReward
        {
            public List<Reward> DailyRewards;
        }
        
        [System.Serializable]
        public class Reward
        {
            public string RewardName;
            public int RewardAmount;
            public Sprite RewardIcon;
        }

        [Tooltip("List of rewards for 7 days")]
        public List<DailyReward> rewards = new List<DailyReward>(7);
        
        public DailyReward GetRewardForDay(int dayIndex)
        {
            if (dayIndex < 1 || dayIndex > rewards.Count)
                dayIndex = 1; // fallback to first day

            return rewards[dayIndex - 1];
        }

        public int TotalDays => rewards.Count;
    }
}