using System;
using DataLayer;
using TMPro;
using UnityEngine;
using Zenject;

namespace VisualLayer.GamePlay.UI
{
    public class GameplayHudUi : MonoBehaviour
    {
        #region Editor

        [SerializeField] 
        private TextMeshProUGUI _coinsBalaceText;

        #endregion

        #region Injects
        
        [Inject]
        private IDataLayer _dataLayer;

        #endregion

        #region Methods

        private void Start()
        {
            InitializeView();
        }

        private void OnDestroy()
        {
            _dataLayer.Balances.CoinsBalanceChanged -= SyncUiWithData;

            SyncUiWithData();
        }

        
        private void InitializeView()
        {
            _dataLayer.Balances.CoinsBalanceChanged+= SyncUiWithData;
            
            SyncUiWithData();
        }
        
        private void SyncUiWithData()
        {
            _coinsBalaceText.text = _dataLayer.Balances.Coins.ToString();
        }

        #endregion
    }
}