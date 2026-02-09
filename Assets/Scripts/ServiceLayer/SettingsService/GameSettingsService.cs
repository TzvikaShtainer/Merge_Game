using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using ServiceLayer.DataSyncService;
using ServiceLayer.MusicService;
using ServiceLayer.PlayFabService;
using UnityEngine;
using Zenject;

namespace ServiceLayer.SettingsService
{
    public class GameSettingsService : IGameSettingsService, ISyncableService
    {
        public event Action OnDataChanged;
        
        [Inject]
        private IServerService _serverService;
        
        [Inject]
        private ISfxService  _sfxService;
        
        [Inject]
        private IMusicService  _musicService;
        
        public GameSettings Settings { get; private set; } = new GameSettings();
        
        private string[] gameSettingsArray = { "HasBGMusic", "HasSFX", "HasVibration" };
        public Dictionary<string, string> GetSyncData()
        {
            return new Dictionary<string, string>
            {
                {"HasBGMusic", Settings.IsMusicOn ? "1" : "0"},
                {"HasSFX", Settings.IsSoundEffectsOn ? "1" : "0"},
                {"HasVibration", Settings.IsVibrationOn ? "1" : "0"}
            };
        }
        public void SetMusic(bool isOn)
        {
            Settings.IsMusicOn  = isOn;
            OnDataChanged?.Invoke();
            
            _musicService.SetMusicEnabled(isOn);
        }

        public void SetSoundEffects(bool isOn)
        {
            Settings.IsSoundEffectsOn  = isOn;
            OnDataChanged?.Invoke();
            
            _sfxService.SetSfxEnabled(isOn);
        }

        public void SetVibration(bool isOn)
        {
            Settings.IsVibrationOn  = isOn;
            OnDataChanged?.Invoke();
        }

        public async UniTask LoadFromServer()
        {
            var data = await _serverService.GetUserData(gameSettingsArray);
            bool shouldSaveDefaults = false;

            if (data.TryGetValue("HasBGMusic", out var bgMusicValue))
            {
                Settings.IsMusicOn = bgMusicValue == "1" || bgMusicValue.Equals("true", StringComparison.OrdinalIgnoreCase);
            }
            else
            {
                Settings.IsMusicOn = true; 
                shouldSaveDefaults = true;
            }
            SetMusic(Settings.IsMusicOn);

            if (data.TryGetValue("HasSFX", out var sfxValue))
            {
                Settings.IsSoundEffectsOn = sfxValue == "1" || sfxValue.Equals("true", StringComparison.OrdinalIgnoreCase);
            }
            else
            {
                Settings.IsSoundEffectsOn = true;
                shouldSaveDefaults = true;
            }
            SetSoundEffects(Settings.IsSoundEffectsOn);

            if (data.TryGetValue("HasVibration", out var vibrationValue))
            {
                Settings.IsVibrationOn = vibrationValue == "1" || vibrationValue.Equals("true", StringComparison.OrdinalIgnoreCase);
            }
            {
                Settings.IsVibrationOn = true; 
                shouldSaveDefaults = true;
            }
            SetVibration(Settings.IsVibrationOn);
            
            if (shouldSaveDefaults)
                OnDataChanged?.Invoke();
        }
    }
}