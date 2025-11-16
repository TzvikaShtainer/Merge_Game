using Cysharp.Threading.Tasks;
using DataLayer;
using ServiceLayer.HourlyCoinsService;
using ServiceLayer.SaveSystem;
using ServiceLayer.SettingsService;
using ServiceLayer.SpinTheWheelCooldownService;
using UnityEngine;
using VisualLayer.GamePlay.Abilities;
using VisualLayer.GamePlay.Popups.DailyRewardPopup;

namespace ServiceLayer.Utilis
{
    public class GameStartupCoordinator
    {
        private IDataLayer  _dataLayer;
        private AbilityManager  _abilityManager;
        private IGameSettingsService   _gameSettingsService;
        private ISaveSystem _saveSystem;
        private IHourlyCoinsService _hourlyCoinsService;
        private ISpinTheWheelCooldownService  _spinTheWheelCooldownService;
        private IDailyRewardCooldownService  _dailyRewardCooldownService;

        public GameStartupCoordinator(IDataLayer  dataLayer, AbilityManager  abilityManager, 
            IGameSettingsService gameSettingsService, ISaveSystem saveSystem
            , IHourlyCoinsService hourlyCoinsService
            , ISpinTheWheelCooldownService spinTheWheelCooldownService
            , IDailyRewardCooldownService  dailyRewardCooldownService)
        {
            _dataLayer =  dataLayer;
            _abilityManager = abilityManager;
            _gameSettingsService = gameSettingsService;
            _saveSystem = saveSystem;
            _hourlyCoinsService = hourlyCoinsService;
            _spinTheWheelCooldownService = spinTheWheelCooldownService;
            _dailyRewardCooldownService = dailyRewardCooldownService;
        }

        public async UniTask LoadAllDataFromServer()
        {
             await _dataLayer.Balances.LoadFromServer();
             await _abilityManager.LoadFromServer();
             await _gameSettingsService.LoadFromServer();
             await _hourlyCoinsService.LoadFromServer();
             await _spinTheWheelCooldownService.LoadFromServer();
             //await _dailyRewardCooldownService.LoadFromServer();
        }

        public async UniTask LoadAllDataFromDevice()
        {
            await _saveSystem.Load();
        }
    }
}