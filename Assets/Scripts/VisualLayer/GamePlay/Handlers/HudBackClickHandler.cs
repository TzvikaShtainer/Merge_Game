using System;
using Cysharp.Threading.Tasks;
using DataLayer.DataTypes;
using ServiceLayer.GameScenes;
using UnityEngine;
using VisualLayer.Loader;
using Zenject;

namespace VisualLayer.GamePlay.Handlers
{
    public class HudBackClickHandler : IHudBackClickHandler
    {
        [Inject]
        private ILoader _loader;
        
        [Inject]
        private IGameScenesService _scenesService;
        
        [Inject]
        private GameLevelType _currentLevelType;
        
        public async UniTask Execute()
        {
            _loader.ResetData();
            await _loader.FadeIn();
            await UniTask.Delay(TimeSpan.FromSeconds(1));
            _loader.SetProgress(0.2f, "20%");
            await UniTask.Delay(TimeSpan.FromSeconds(1));
            _loader.SetProgress(0.5f, "50%");
            
            //unload gameplay lvl scene
            await _scenesService.UnloadLevelScene(_currentLevelType);
            
            await UniTask.Delay(TimeSpan.FromSeconds(1));
            _loader.SetProgress(1f, "100%");
            
            //load Start Screen scene
            await _scenesService.LoadLevelSceneIfNotLoaded(GameLevelType.StartScreen);
            
            
            await UniTask.Delay(TimeSpan.FromSeconds(1));
            await _loader.FadeOut();
        }
    }
}