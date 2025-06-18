using Cysharp.Threading.Tasks;
using DataLayer;
using ServiceLayer.SaveSystem;
using ServiceLayer.SettingsService;
using UnityEngine;
using VisualLayer.GamePlay.Abilities;

namespace ServiceLayer.Utilis
{
    public class GameStartupCoordinator
    {
        private IDataLayer  _dataLayer;
        private AbilityManager  _abilityManager;
        private IGameSettingsService   _gameSettingsService;
        private ISaveSystem _saveSystem;

        public GameStartupCoordinator(IDataLayer  dataLayer, AbilityManager  abilityManager, 
            IGameSettingsService   gameSettingsService, ISaveSystem saveSystem)
        {
            _dataLayer =  dataLayer;
            _abilityManager = abilityManager;
            _gameSettingsService = gameSettingsService;
            _saveSystem = saveSystem;
        }

        public async UniTask LoadAllDataFromServer()
        {
             await _dataLayer.Balances.LoadFromServer();
             await _abilityManager.LoadFromServer();
             await _gameSettingsService.LoadFromServer();
             await _saveSystem.Load();
        }
    }
}