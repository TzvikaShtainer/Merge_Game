    using System;
    using Cysharp.Threading.Tasks;
    using DataLayer;
    using DataLayer.DataTypes;
    using ServiceLayer.GameScenes;
    using ServiceLayer.Signals.SignalsClasses;
    using ServiceLayer.TimeControl;
    using UnityEngine;
    using VisualLayer.GamePlay.Abilities;
    using VisualLayer.GamePlay.Popups.YesNoPopup;
    using VisualLayer.Loader;
    using Zenject;

    namespace VisualLayer.GamePlay.Handlers
    {
        public class GameEndHandler
        {
            [Inject]
            private ILoader _loader;
            
            [Inject] 
            private YesNoPopup.Factory _yesNoPopupFactory;
            
            [Inject]
            private ITimeController _timeController;
            
            [Inject]
            private IGameScenesService _scenesService;

            [Inject]
            private GameLevelType _currentLevelType;
            
            [Inject]
            private SignalBus _signalBus;
            
            [Inject]
            private AbilityManager _abilityManager;
            
            [Inject]
            private IDataLayer  _dataLayer;
            
            public async void Execute()
            {
                _signalBus.Fire<PauseInput>();

                var popupArgs = new YesNoPopupArgs
                {
                    Text = "You Lose!",
                    IsNoButtonVisible = true,
                    YesCaption = "Try Again?",
                    NoCaption = "Go Home",
                };
                
                var popup = _yesNoPopupFactory.Create(popupArgs);
                
                await UniTask.Delay(TimeSpan.FromSeconds(0.2f));
                
                _timeController.PauseGameplay();
                
                var result = await popup.WaitForResult();
                
                _timeController.UnpauseGameplay();
                
                if (result.IsYes)
                {
                    _signalBus.Fire<UnpauseInput>();
                    _signalBus.Fire<OnContinueClicked>();
                    
                    _abilityManager.UseAbility("DestroyItemsAfterContinue");
                    _dataLayer.Balances.RemoveCoins(20);
                    await UniTask.Delay(TimeSpan.FromSeconds(0.5));
                }
                else
                {
                    _signalBus.Fire<UnpauseInput>();
                
                    _loader.ResetData();
                    await _loader.FadeIn();
                    await UniTask.Delay(TimeSpan.FromSeconds(1));
                    _loader.SetProgress(0.2f, "Loading Level 20%");
                    await UniTask.Delay(TimeSpan.FromSeconds(1));
                    _loader.SetProgress(0.5f, "Loading Level 50%");
                
                    //unload gameplay lvl scene
                    await _scenesService.UnloadLevelScene(_currentLevelType);
                
                    await UniTask.Delay(TimeSpan.FromSeconds(1));
                    _loader.SetProgress(1f, "Loading Level 100%");
                
                    //load Start Screen scene
                    await _scenesService.LoadLevelSceneIfNotLoaded(GameLevelType.GamePlay);
                
                
                    await UniTask.Delay(TimeSpan.FromSeconds(1));
                    await _loader.FadeOut();
                }
            }
        }
    }