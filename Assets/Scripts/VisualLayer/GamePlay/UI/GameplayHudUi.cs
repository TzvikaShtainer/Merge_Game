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
        private Image _nextItemSprite;

        #endregion

        #region Injects
        
        [Inject]
        private IDataLayer _dataLayer;
        
        [Inject]
        private IHudBackClickHandler _backClickHandler; 
        
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
            _gameLogicHandler.NextItemCreated -= SyncUiWithData;

            SyncUiWithData();
        }

        
        private void InitializeView()
        {
            _dataLayer.Balances.CoinsBalanceChanged += SyncUiWithData;
            _gameLogicHandler.NextItemCreated += SyncUiWithData;
            
            SyncUiWithData();
        }
        
        private void SyncUiWithData()
        {
            _higestScoreText.text = _dataLayer.Balances.Coins.ToString();
            
            _coinsBalaceText.text = _dataLayer.Balances.Coins.ToString();
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
        
        #endregion
    }
}