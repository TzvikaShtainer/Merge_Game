using System;
using Cysharp.Threading.Tasks;
using DataLayer;
using DataLayer.DataTypes;
using DG.Tweening;
using ServiceLayer.GameScenes;
using ServiceLayer.SettingsService;
using ServiceLayer.Signals.SignalsClasses;
using TMPro;
using UnityEngine;
using VisualLayer.GamePlay.Popups.YesNoPopup;
using VisualLayer.Loader;
using Zenject;
using Button = UnityEngine.UIElements.Button;
namespace VisualLayer.GamePlay.Popups.MusicMenuPopup
{
    public class SettingsMenuPopup : Popup
    {
        #region Factories

        public class Factory : PlaceholderFactory<SettingsMenuPopup>
        {
        }

        #endregion

        #region Editor

        [SerializeField] private ToggleButton _musicToggleButton;
        [SerializeField] private ToggleButton _sfxToggleButton;
        [SerializeField] private ToggleButton _vibrationToggleButton;

        #endregion

        #region Fields

        private ISettingsMenuActions _settingsMenuActions;

        #endregion

        #region Injects

        [Inject] private IGameSettingsService _gameSettingsService;

        #endregion

        #region Methods

        public void Initialize(ISettingsMenuActions settingsMenuActions)
        {
            _settingsMenuActions = settingsMenuActions;
            _musicToggleButton.SetState(_gameSettingsService.Settings.IsMusicOn);
            _sfxToggleButton.SetState(_gameSettingsService.Settings.IsSoundEffectsOn);
            _vibrationToggleButton.SetState(_gameSettingsService.Settings.IsVibrationOn);
        }

        public void OnContinueBtnClick() => Close();


        public void OnBgMusicBtnClick()
        {
            _musicToggleButton.Toggle();
            _settingsMenuActions.OnToggleMusic();
        }

        public void OnSoundBtnClick()
        {
            _sfxToggleButton.Toggle();
            _settingsMenuActions.OnToggleSfx();
        }

        public void OnVibrationBtnClick()
        {
            _vibrationToggleButton.Toggle();
            _settingsMenuActions.OnToggleVibration();
        }

        public void OnRestartBtnClick()
        {
            Close();
            _settingsMenuActions.OnRestartGame();
        }

        #endregion
    }
}