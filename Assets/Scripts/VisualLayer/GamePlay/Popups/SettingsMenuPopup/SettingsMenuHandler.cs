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
           
        }

        public void OnToggleSfx()
        {
            
        }

        public void OnToggleVibration()
        {
            
        }

        public async UniTask OnRestartGame()
        {
            _loader.ResetData();
            await _loader.FadeIn();
            _loader.SetProgress(0.2f, "Loading Level 20%");
            await UniTask.Delay(TimeSpan.FromSeconds(1));

            
            //unload gameplay lvl scene
            _loader.SetProgress(0.5f, "Loading Level 50%");
            await _scenesService.UnloadLevelScene(_currentLevelType);
            await UniTask.Delay(TimeSpan.FromSeconds(1));
            
            //load lvl selection scene
            _loader.SetProgress(0.7f, "Loading Level 70%");
            await _scenesService.LoadLevelSceneIfNotLoaded(GameLevelType.GamePlay);
            
            await _scenesService.LoadInfraSceneIfNotLoaded(InfraScreenType.Loader);
            await _scenesService.LoadInfraSceneIfNotLoaded(InfraScreenType.GamePopups);
            
            await UniTask.Delay(TimeSpan.FromSeconds(1));
            _loader.SetProgress(1f, "Loading Level 100%");
            await UniTask.Delay(TimeSpan.FromSeconds(1));
            await _loader.FadeOut();
        }
    }
}