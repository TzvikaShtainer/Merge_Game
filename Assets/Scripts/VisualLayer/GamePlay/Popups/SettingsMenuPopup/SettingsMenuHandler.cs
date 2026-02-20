using System;
using Cysharp.Threading.Tasks;
using DataLayer;
using DataLayer.DataTypes;
using ServiceLayer.GameScenes;
using ServiceLayer.MusicService;
using ServiceLayer.SettingsService;
using ServiceLayer.Signals.SignalsClasses;
using VisualLayer.GamePlay.Popups.YesNoPopup;
using VisualLayer.Loader;
using Zenject;

namespace VisualLayer.GamePlay.Popups.MusicMenuPopup
{
    public class SettingsMenuHandler : ISettingsMenuActions
    {
        [Inject]
        private ILoader _loader;
        
        [Inject]
        private IGameScenesService _scenesService;
        
        [Inject]
        private GameLevelType _currentLevelType;
        
        [Inject]
        private IGameSettingsService _gameScenesService;
        
        [Inject]
        private IDataLayer _dataLayer;
        
        [Inject]
        private ISfxService  _sfxService;
        
        [Inject]
        private SignalBus _signalBus;
        
        [Inject] 
        private global::VisualLayer.GamePlay.Popups.YesNoPopup.YesNoPopup.Factory _yesNoPopupFactory;
        
        public void OnToggleMusic()
        {
            bool isOn = !_gameScenesService.Settings.IsMusicOn;
            _gameScenesService.SetMusic(isOn);
            
            _sfxService.PlaySfxType(SfxType.Click);
        }

        public void OnToggleSfx()
        {
            bool isOn = !_gameScenesService.Settings.IsSoundEffectsOn;
            _gameScenesService.SetSoundEffects(isOn);
            
            _sfxService.PlaySfxType(SfxType.Click);
        }

        public void OnToggleVibration()
        {
            bool isOn = !_gameScenesService.Settings.IsVibrationOn;
            _gameScenesService.SetVibration(isOn);
            
            _sfxService.PlaySfxType(SfxType.Click);
        }

        public async UniTask OnRestartGame()
        { 
            _sfxService.PlaySfxType(SfxType.Click);

            var popupArgs = new YesNoPopupArgs
            {
                Text = "Are You Sure?",
                IsNoButtonVisible = true,
                YesCaption = "Yes",
                NoCaption = "No"
            };
            
            var popup = _yesNoPopupFactory.Create(popupArgs);
            await UniTask.Delay(TimeSpan.FromSeconds(0.2f));
            var result = await popup.WaitForResult();

            if (result.IsNo)
            {
                _signalBus.Fire<UnpauseInputSignal>();
                await UniTask.Delay(TimeSpan.FromSeconds(0.5));
            }

            else
            {
                await _loader.InitLoader();
            
                await UniTask.Delay(TimeSpan.FromSeconds(1));
                await _loader.AnimateProgressTo(0.2f, 0.3f);
            
                await _loader.AnimateProgressTo(0.5f, 0.5f);

                await _scenesService.UnloadLevelScene(_currentLevelType);
                await UniTask.Delay(TimeSpan.FromSeconds(1));
            
                await _loader.AnimateProgressTo(0.7f, 0.5f);
           
                _dataLayer.Balances.SetCurrentScore(0);
            
                await _scenesService.LoadLevelSceneIfNotLoaded(GameLevelType.GamePlay);
            
                await _scenesService.LoadInfraSceneIfNotLoaded(InfraScreenType.Loader);
                await _scenesService.LoadInfraSceneIfNotLoaded(InfraScreenType.GamePopups);
            
                await UniTask.Delay(TimeSpan.FromSeconds(1));
                //_loader.SetProgress(1f, "Loading Level 100%");
                await _loader.AnimateProgressTo(1f, 0.5f);

                await UniTask.Delay(TimeSpan.FromSeconds(1));
                await _loader.FadeOut();
            
                _signalBus.Fire<UnpauseInputSignal>();
            }
        }
    }
}