using System;
using DataLayer;
using ServiceLayer.MusicService;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VisualLayer.GamePlay.Abilities;
using VisualLayer.GamePlay.Handlers;
using VisualLayer.GamePlay.Popups.AddSkillsPopup;
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
        private ISettingsMenuClickHandler _settingsClickHandler; 
        
        [Inject]
        private IBackClickHandler _backClickHandler; 
        
        [Inject]
        private IHudPlusCurrencyClickHandler _plusCurrencyClickHandler; 
        
        [Inject]
        private IGameLogicHandler _gameLogicHandler;
        
        [Inject]
        private AbilityManager _abilityManager;
        
        [Inject]
        private ISfxService  _sfxService;
        
        [Inject]
        private AddSkillsPopup.Factory _addSkillsPopupFactory;

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
            
            //HandleCurrentScore();

            SyncUiWithData();

            SyncAbilityCountData("DestroyAllLowestLevelFruitsAbility", _abilityManager.GetAbilityCount("DestroyAllLowestLevelFruitsAbility"));
            SyncAbilityCountData("DestroySpecificFruitAbility", _abilityManager.GetAbilityCount("DestroySpecificFruitAbility"));
            SyncAbilityCountData("ShakeBoxAbility", _abilityManager.GetAbilityCount("ShakeBoxAbility"));
            SyncAbilityCountData("UpgradeSpecificFruitAbility", _abilityManager.GetAbilityCount("UpgradeSpecificFruitAbility"));
        }

        private void HandleCurrentScore()
        {
            int currentScore = _dataLayer.Balances.GetCurrentScore();
            if (currentScore > 0)
            {
                _currentScoreText.text = currentScore.ToString();
            }
            else
            {
                _dataLayer.Balances.SetCurrentScore(0);
                _currentScoreText.text = "0";
            }
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
        
        public async void OnSettingsButtonClick()
        {
            _settingsClickHandler.Execute();
            _sfxService.PlaySfxType(SfxType.Click);
        }
        
        public async void OnBackButtonClick()
        {
            _backClickHandler.Execute();
            _sfxService.PlaySfxType(SfxType.Click);
        }

        public void OnAbilityButtonClick(string abilityId)
        {
            //Debug.Log("abilityId: "+ abilityId +"firstTime: "+ _abilityManager.IsAbilityFirstTime(abilityId));
            if (_abilityManager.GetAbilityCount(abilityId) == 0)
            {
                _sfxService.PlaySfxType(SfxType.OpenPopup);
                
                _plusCurrencyClickHandler.Execute(abilityId);
            }
            else
            {
                _abilityManager.UseAbility(abilityId);
            }

        }

        public void OnPlusButtonClick(string abilityId)
        {
            _plusCurrencyClickHandler.Execute(abilityId);
            _sfxService.PlaySfxType(SfxType.Click);
        }
        
        #endregion
    }
}