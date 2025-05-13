using System;
using Cysharp.Threading.Tasks;
using DataLayer;
using DataLayer.DataTypes;
using DG.Tweening;
using ServiceLayer.GameScenes;
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

        public class Factory : PlaceholderFactory<SettingsMenuPopup> { }

        #endregion

        #region Editor

        [SerializeField] private ToggleButton _musicToggle;
        [SerializeField] private ToggleButton _sfxToggle;
        [SerializeField] private ToggleButton _vibrationToggle;
        
        #endregion
        
        #region Fields
        
        private ISettingsMenuActions _settingsMenuActions;

        #endregion

        #region Injects

        #endregion

        #region Methods
        
        public void Initialize(ISettingsMenuActions settingsMenuActions)
        {
            _settingsMenuActions = settingsMenuActions;
        }
        
        public void OnContinueBtnClick() => Close();
        

        public void OnBgMusicBtnClick()
        {
            _musicToggle.Toggle();
            _settingsMenuActions.OnToggleMusic();
        }

        public void OnSoundBtnClick()
        {
            _sfxToggle.Toggle();
            _settingsMenuActions.OnToggleSfx();
        }

        public void OnVibrationBtnClick()
        {
            _vibrationToggle.Toggle();
            _settingsMenuActions.OnToggleVibration();
        }

        public async void OnRestartBtnClick()
        {
            _settingsMenuActions.OnRestartGame();
            
            Close();
        }

        #endregion
    }
}