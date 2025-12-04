using Cysharp.Threading.Tasks;
using DataLayer;
using DataLayer.DataTypes;
using ServiceLayer.GameScenes;
using ServiceLayer.SaveSystem;
using ServiceLayer.Signals.SignalsClasses;
using UnityEngine;
using VisualLayer.Loader;
using Zenject;

namespace VisualLayer.GamePlay.Handlers
{
    public class BackClickHandler : IBackClickHandler
    {
        [Inject]
        private ILoader _loader;
        
        [Inject]
        private IGameScenesService _scenesService;
        
        [Inject]
        private SignalBus _signalBus;
        
        [Inject] 
        private ISaveSystem _saveService;
        
        [Inject]
        private IDataLayer _dataLayer;
        public async UniTask Execute()
        {
            _saveService.Save();
            _dataLayer.Balances.SetCurrentScore(_dataLayer.Balances.CurrentScore);
            
            await _loader.InitLoader();
            
            await _loader.AnimateProgressTo(0.2f, 0.5f);
            
            await UniTask.Delay(1000);
            
            await _scenesService.UnloadLevelScene(GameLevelType.GamePlay);
            
            await _loader.AnimateProgressTo(0.5f, 1f);
            
            await _scenesService.LoadLevelSceneIfNotLoaded(GameLevelType.StartScreen);
            
            await _loader.AnimateProgressTo(1f, 0.5f);
            
            await _loader.FadeOut();
            
            _signalBus.Fire<UnpauseInputSignal>();
            
            await UniTask.Delay(500);
            
            _signalBus.Fire<UIComponentsInBehaviorSignal>();
        }
    }
}