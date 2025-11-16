using DataLayer;
using EasyUI.PickerWheelUI;
using UnityEngine;
using VisualLayer.GamePlay.Abilities;
using Zenject;

namespace VisualLayer.GamePlay.RewardSystem
{
    public enum RewardType
    {
        Coins,
        Skill
    }
    
    public class WheelRewardSystem : IRewardSystem
    {
        [Inject]
        private IDataLayer  _dataLayer;
        
        [Inject]
        private AbilityManager  _abilityManager;

        public void GrandReward(WheelPiece wheelPiece)
        {
            switch(wheelPiece.rewardType)
            {
                case RewardType.Coins:
                    _dataLayer.Balances.AddCoins(wheelPiece.Amount);
                    //Winning Sound
                    break;

                case RewardType.Skill:
                    _abilityManager.AddAbilityCount(wheelPiece.Label, wheelPiece.Amount);
                    break;
            }
            
                
        }
    }
}