using System;
using DataLayer;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VisualLayer.GamePlay.Abilities;
using VisualLayer.GamePlay.Handlers;
using Zenject;

namespace VisualLayer.GamePlay.UI
{
    public class GameplayHudUi : MonoBehaviour
    {
        #region Editor

        [SerializeField] 
        private TextMeshProUGUI _coinsBalaceText;

        [SerializeField] 
        private TextMeshProUGUI _higestScoreText;
        
        [SerializeField] 
        private TextMeshProUGUI _currentScoreText;
        
        [SerializeField] 
        private Image _nextItemSprite;
        
        [SerializeField] 
        private TextMeshProUGUI _destroyAllLowestLevelFruitsAbilityText;
        
        [SerializeField] 
        private TextMeshProUGUI _destroySpecificFruitAbilityText;
        
        [SerializeField] 
        private TextMeshProUGUI _shakeBoxAbilityText;
        
        [SerializeField] 
        private TextMeshProUGUI _upgradeSpecificFruitAbilityText;

        #endregion

        #region Injects
        
        [Inject]
        private IDataLayer _dataLayer;
        
        [Inject]
        private ISettingsMenuClickHandler _backClickHandler; 
        
        [Inject]
        private IHudPlusCurrencyClickHandler _plusCurrencyClickHandler; 
        
        [Inject]
        private IGameLogicHandler _gameLogicHandler;
        
        [Inject]
        private AbilityManager _abilityManager;

        #endregion

        #region Methods

        private void Start()
        {
            InitializeView();
        }

        private void OnDestroy()
        {
            _dataLayer.Balances.CoinsBalanceChanged -= SyncUiWithData;
            _dataLayer.Balances.HighScoreChanged -= SyncUiWithData;
            _dataLayer.Balances.ScoreChanged -= SyncUiWithData;
            
            _gameLogicHandler.NextItemCreated -= SyncUiWithData;
            
            _abilityManager.OnAbilityChanged -= SyncAbilityCountData;

            SyncUiWithData();
        }

        
        private void InitializeView()
        {
            _dataLayer.Balances.CoinsBalanceChanged += SyncUiWithData;
            _dataLayer.Balances.HighScoreChanged += SyncUiWithData;
            _dataLayer.Balances.ScoreChanged += SyncUiWithData;
            
            _gameLogicHandler.NextItemCreated += SyncUiWithData;

            _abilityManager.OnAbilityChanged += SyncAbilityCountData;
            
            _currentScoreText.text = "0";
            
            _dataLayer.Balances.SetCurrentScore(0);
            
            SyncUiWithData();

            SyncAbilityCountData("DestroyAllLowestLevelFruitsAbility", _abilityManager.GetAbilityCount("DestroyAllLowestLevelFruitsAbility"));
            SyncAbilityCountData("DestroySpecificFruitAbility", _abilityManager.GetAbilityCount("DestroySpecificFruitAbility"));
            SyncAbilityCountData("ShakeBoxAbility", _abilityManager.GetAbilityCount("ShakeBoxAbility"));
            SyncAbilityCountData("UpgradeSpecificFruitAbility", _abilityManager.GetAbilityCount("UpgradeSpecificFruitAbility"));
        }

        private void SyncUiWithData()
        {
            _coinsBalaceText.text = _dataLayer.Balances.Coins.ToString();
            
            _higestScoreText.text = _dataLayer.Balances.HighScore.ToString();
            
            _currentScoreText.text = _dataLayer.Balances.CurrentScore.ToString();
            
            _nextItemSprite.sprite = _gameLogicHandler.GetNextItem().GetItemMetadata().ItemPreviewSprite;
        }

        private void SyncAbilityCountData(string abilityId, int newCount)
        {
            if (abilityId == "DestroyAllLowestLevelFruitsAbility") _destroyAllLowestLevelFruitsAbilityText.text = newCount.ToString();
            if (abilityId == "DestroySpecificFruitAbility") _destroySpecificFruitAbilityText.text = newCount.ToString();
            if (abilityId == "ShakeBoxAbility") _shakeBoxAbilityText.text = newCount.ToString();
            if (abilityId == "UpgradeSpecificFruitAbility") _upgradeSpecificFruitAbilityText.text = newCount.ToString();
        }
        
        public async void OnBackButtonClick()
        {
            _backClickHandler.Execute();
        }

        public void OnAbilityButtonClick(string abilityId)
        {
            _abilityManager.UseAbility(abilityId);
        }

        public void OnPlusButtonClick(string abilityId)
        {
            
            _plusCurrencyClickHandler.Execute(abilityId);
        }
        
        #endregion
    }
}