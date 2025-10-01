using DataLayer;
using TMPro;
using UnityEngine;
using Zenject;

namespace VisualLayer.GamePlay.UI
{
    public class StartScreenHudUI : MonoBehaviour
    {
        [SerializeField] 
        private TextMeshProUGUI _coinsBalanceText;
        
        [Inject]
        private IDataLayer _dataLayer;
        
        private void Start()
        {
            InitializeView();
        }
        private void InitializeView()
        {
            _dataLayer.Balances.CoinsBalanceChanged += SyncUiWithData;
        }

        private void SyncUiWithData()
        {
            _coinsBalanceText.text = _dataLayer.Balances.Coins.ToString();
        }
    }
}