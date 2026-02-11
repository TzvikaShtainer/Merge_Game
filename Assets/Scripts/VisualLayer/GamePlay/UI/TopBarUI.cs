using DataLayer;
using ServiceLayer.MusicService;
using TMPro;
using UnityEngine;
using VisualLayer.GamePlay.Handlers;
using Zenject;

namespace VisualLayer.GamePlay.UI
{
    public class TopBarUI : MonoBehaviour
    {
        [SerializeField] 
        private TextMeshProUGUI _coinsBalanceText;
        
        [Inject]
        private IDataLayer _dataLayer;
        
        [Inject]
        private ISettingsMenuClickHandler _settingsClickHandler; 
        
        private void Start()
        {
            InitializeView();
        }
        private void InitializeView()
        {
            _dataLayer.Balances.CoinsBalanceChanged += SyncUiWithData;
            
            SyncUiWithData();
        }

        private void OnDestroy()
        {
            _dataLayer.Balances.CoinsBalanceChanged -= SyncUiWithData;
        }

        private void SyncUiWithData()
        {
            _coinsBalanceText.text = _dataLayer.Balances.Coins.ToString();
        }
        
        public async void OnSettingsButtonClick()
        {
            //_settingsClickHandler.Execute();
            //_sfxService.PlaySfxType(SfxType.Click);
        }
    }
}