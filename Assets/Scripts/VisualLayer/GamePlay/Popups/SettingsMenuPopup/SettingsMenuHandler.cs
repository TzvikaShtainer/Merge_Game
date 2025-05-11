using System;
using Cysharp.Threading.Tasks;
using DataLayer.DataTypes;
using ServiceLayer.GameScenes;
using UnityEngine;
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
        
        public void OnToggleMusic()
        {
           Debug.Log("OnToggleMusic");
        }

        public void OnToggleSfx()
        {
            Debug.Log("OnToggleSfx");
        }

        public void OnToggleVibration()
        {
            Debug.Log("OnToggleVibration");
        }

        public async UniTask OnRestartGame()
        {
            _loader.ResetData();
            await _loader.FadeIn();
            _loader.SetProgress(0.1f, "Going to the level selection");
            await UniTask.Delay(TimeSpan.FromSeconds(1));

            
            //unload gameplay lvl scene
            _loader.SetProgress(0.2f, "Unloading the level");
            await _scenesService.UnloadLevelScene(_currentLevelType);
            await UniTask.Delay(TimeSpan.FromSeconds(1));
            
            //load lvl selection scene
            _loader.SetProgress(0.4f, "Loading levels list");
            await _scenesService.LoadLevelSceneIfNotLoaded(GameLevelType.GamePlay);
            
            await _scenesService.LoadInfraSceneIfNotLoaded(InfraScreenType.Loader);
            await _scenesService.LoadInfraSceneIfNotLoaded(InfraScreenType.GamePopups);
            
            await UniTask.Delay(TimeSpan.FromSeconds(1));
            _loader.SetProgress(0.9f, "Completing");
            await UniTask.Delay(TimeSpan.FromSeconds(1));
            await _loader.FadeOut();
        }

        public void OnCloseMenu()
        {
            Debug.Log("OnCloseMenu");
        }
    }
}