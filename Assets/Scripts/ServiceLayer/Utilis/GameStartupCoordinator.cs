using Cysharp.Threading.Tasks;
using DataLayer;
using ServiceLayer.DataSyncService;
using ServiceLayer.HourlyCoinsService;
using ServiceLayer.NotificationsService;
using ServiceLayer.SaveSystem;
using ServiceLayer.SettingsService;
using ServiceLayer.Signals.SignalsClasses;
using ServiceLayer.SpinTheWheelCooldownService;
using UnityEngine;
using VisualLayer.GamePlay.Abilities;
using VisualLayer.GamePlay.Popups.DailyRewardPopup;
using Zenject;

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
        private readonly DataSyncService.DataSyncService _syncService;
        
        [Inject]
        private SignalBus  _signalBus;

        public GameStartupCoordinator(
            IDataLayer dataLayer, 
            AbilityManager abilityManager, 
            IGameSettingsService gameSettingsService, 
            ISaveSystem saveSystem,
            IHourlyCoinsService hourlyCoinsService,
            ISpinTheWheelCooldownService spinTheWheelCooldownService,
            IDailyRewardCooldownService dailyRewardCooldownService,
            DataSyncService.DataSyncService syncService)
        {
            _dataLayer = dataLayer;
            _abilityManager = abilityManager;
            _gameSettingsService = gameSettingsService;
            _saveSystem = saveSystem;
            _hourlyCoinsService = hourlyCoinsService;
            _spinTheWheelCooldownService = spinTheWheelCooldownService;
            _dailyRewardCooldownService = dailyRewardCooldownService;
            _syncService = syncService;
        }
        
        public async UniTask LoadLocalAndServer()
        {
            await LoadAllDataFromServer();
            await LoadAllDataFromDevice();
        }

        public async UniTask LoadAllDataFromServer()
        {
            
            var syncLock = _syncService as ISyncLock; 
    
            syncLock?.LockSync(); 

            try 
            {
                await UniTask.WhenAll(
                    _dataLayer.Balances.LoadFromServer(),
                    _gameSettingsService.LoadFromServer(),
                    _hourlyCoinsService.LoadFromServer(),
                    _spinTheWheelCooldownService.LoadFromServer(),
                    _dailyRewardCooldownService.LoadFromServer(),
                    _abilityManager.LoadFromServer()
                );

                await UniTask.Yield();
            }
            catch (System.Exception e)
            {
                Debug.LogError($"[GameStartupCoordinator] Error during data load: {e.Message}");
            }
            finally 
            {
                syncLock?.UnlockSync();
            }
        }

        public async UniTask LoadAllDataFromDevice()
        {
            await _saveSystem.Load();
        }
    }
}