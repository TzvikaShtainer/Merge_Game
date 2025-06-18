using DataLayer;
using ServiceLayer.SaveSystem;
using ServiceLayer.SettingsService;
using VisualLayer.GamePlay.Abilities;

namespace ServiceLayer.Utilis
{
    public class GameStartupCoordinator
    {
        private IDataLayer  _dataLayer;
        private AbilityManager  _abilityManager;
        private IGameSettingsService   _gameSettingsService;
        private ISaveSystem _saveSystem;
    }
}