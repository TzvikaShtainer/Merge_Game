using System;
using DataLayer;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
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
        private Image _nextItemSprite;

        #endregion

        #region Injects
        
        [Inject]
        private IDataLayer _dataLayer;
        
        [Inject]
        private IHudBackClickHandler _backClickHandler; 
        
        [Inject]
        private IGameLogicHandler _gameLogicHandler;

        #endregion

        #region Methods

        private void Start()
        {
            InitializeView();
        }

        private void OnDestroy()
        {
            _dataLayer.Balances.CoinsBalanceChanged -= SyncUiWithData;
            //_gameLogicHandler.NextItemCreated -= SyncUiWithData;

            SyncUiWithData();
        }

        
        private void InitializeView()
        {
            _dataLayer.Balances.CoinsBalanceChanged += SyncUiWithData;
            //_gameLogicHandler.NextItemCreated += SyncUiWithData;
            
            SyncUiWithData();
        }
        
        private void SyncUiWithData()
        {
            _coinsBalaceText.text = _dataLayer.Balances.Coins.ToString();
           // _nextItemSprite.sprite = _gameLogicHandler.GeNextItem().GetItemMetadata().ItemPreviewSprite;
        }

        
        public async void OnBackButtonClick()
        {
            _backClickHandler.Execute();
        }
        
        #endregion
    }
}