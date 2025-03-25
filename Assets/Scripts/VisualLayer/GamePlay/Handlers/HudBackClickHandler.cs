using System;
using Cysharp.Threading.Tasks;
using DataLayer.DataTypes;
using ServiceLayer.GameScenes;
using ServiceLayer.Signals.SignalsClasses;
using ServiceLayer.TimeControl;
using UnityEngine;
using VisualLayer.GamePlay.Popups.YesNoPopup;
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

        [Inject] 
        private YesNoPopup.Factory _yesNoPopupFactory;
        
        [Inject]
        private ITimeController _timeController;
        
        [Inject]
        private SignalBus _signalBus;
        
        public async UniTask Execute()
        {
            _timeController.PauseGameplay();
            _signalBus.Fire<PauseInput>();
            
            var popup = _yesNoPopupFactory.Create(YesNoPopupArgs.Default);
            var result = await popup.WaitForResult();
            
            _timeController.UnpauseGameplay();

            if (result.IsNo)
            {
                _signalBus.Fire<UnpauseInput>();
                return;
            }
            
            _signalBus.Fire<UnpauseInput>();
            
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