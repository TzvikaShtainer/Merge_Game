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

        #endregion

        #region Injects
        
        [Inject]
        private IDataLayer _dataLayer;
        
        [Inject]
        private IHudBackClickHandler _backClickHandler; 
        
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

            SyncUiWithData();
        }

        
        private void InitializeView()
        {
            _dataLayer.Balances.CoinsBalanceChanged += SyncUiWithData;
            _dataLayer.Balances.HighScoreChanged += SyncUiWithData;
            _dataLayer.Balances.ScoreChanged += SyncUiWithData;
            _gameLogicHandler.NextItemCreated += SyncUiWithData;
            
            _currentScoreText.text = "0";
            
            _dataLayer.Balances.SetCurrentScore(0);
            
            SyncUiWithData();
        }
        
        private void SyncUiWithData()
        {
            _coinsBalaceText.text = _dataLayer.Balances.Coins.ToString();
            
            _higestScoreText.text = _dataLayer.Balances.HighScore.ToString();
            
            _currentScoreText.text = _dataLayer.Balances.CurrentScore.ToString();
            
            _nextItemSprite.sprite = _gameLogicHandler.GetNextItem().GetItemMetadata().ItemPreviewSprite;
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