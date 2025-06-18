using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using ServiceLayer.PlayFabService;
using UnityEngine;
using Zenject;

namespace ServiceLayer.SettingsService
{
    public class GameSettingsService : IGameSettingsService
    {
        [Inject]
        private IServerService _serverService;
        
        public GameSettings Settings { get; private set; } = new GameSettings();
        
        private string[] gameSettingsArray = { "HasBGMusic", "HasSFX", "HasVibration" };
        public void SetMusic(bool isOn)
        {
            Settings.IsMusicOn  = isOn;
            Save("HasBGMusic",  isOn);
        }

        public void SetSoundEffects(bool isOn)
        {
            Settings.IsSoundEffectsOn  = isOn;
            Save("HasSFX",  isOn);
        }

        public void SetVibration(bool isOn)
        {
            Settings.IsVibrationOn  = isOn;
            Save("HasVibration",  isOn);
        }

        public async UniTask LoadFromServer()
        {
            var data = await _serverService.GetUserData(gameSettingsArray);
            bool shouldSaveDefaults = false;
            
            if (data.TryGetValue("HasBGMusic", out var bgMusicValue))
                Settings.IsMusicOn = bgMusicValue == "1" || bgMusicValue.Equals("true", StringComparison.OrdinalIgnoreCase);
            else
                shouldSaveDefaults = true;

            if (data.TryGetValue("HasSFX", out var sfxValue))
                Settings.IsSoundEffectsOn = sfxValue == "1" || sfxValue.Equals("true", StringComparison.OrdinalIgnoreCase);
            else
                shouldSaveDefaults = true;

            if (data.TryGetValue("HasVibration", out var vibrationValue))
                Settings.IsVibrationOn = vibrationValue == "1" || vibrationValue.Equals("true", StringComparison.OrdinalIgnoreCase);
            else
                shouldSaveDefaults = true;

            if (shouldSaveDefaults)
                SaveAll();
        }

        private void Save(string key, bool value)
        {
            _serverService.SetUserData(new Dictionary<string, string>
            {
                { key,  value ? "1" : "0" }
            }).Forget();
         }
        
        private void SaveAll()
        {
            _serverService.SetUserData(new Dictionary<string, string>
            {
                { "HasBGMusic", Settings.IsMusicOn ? "1" : "0" },
                { "HasSFX", Settings.IsSoundEffectsOn ? "1" : "0" },
                { "HasVibration", Settings.IsVibrationOn ? "1" : "0" }
            }).Forget();
        }
    }
}