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
    using Cysharp.Threading.Tasks;
    using Cysharp.Threading.Tasks.Linq;
    using ServiceLayer.SaveSystem;

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
            
            [Inject] 
            private ISaveSystem _saveSystem;

            private int _coinsForTryAgain = 20;
            
            public async void Execute()
            {
                _signalBus.Fire<PauseInputSignal>();
                
                if (_loader.IsActive) 
                {
                    await UniTask.WaitUntil(() => !_loader.IsActive);
                }

                var popupArgs = new YesNoPopupArgs
                {
                    Text = "You Lose!",
                    IsNoButtonVisible = true,
                    YesCaption = "Go Home",
                    NoCaption = $"Try Again\n({_coinsForTryAgain} Coins)",
                };
                
                if (_dataLayer.Balances.GetCurrentCoins() < _coinsForTryAgain)
                    popupArgs.IsNoButtonVisible =  false;
                
                var popup = _yesNoPopupFactory.Create(popupArgs);
                
                await UniTask.Delay(TimeSpan.FromSeconds(0.2f));
                
                _timeController.PauseGameplay();
                
                var result = await popup.WaitForResult();
                
                _timeController.UnpauseGameplay();
                
                if (result.IsNo)
                {
                    _abilityManager.UseAbility("DestroyItemsAfterContinue");
                    _dataLayer.Balances.RemoveCoins(_coinsForTryAgain);
                    await UniTask.Delay(TimeSpan.FromSeconds(0.5));

                    _signalBus.Fire<OnContinueClickedSignal>();
                    _signalBus.Fire<UnpauseInputSignal>();
                }
                else
                {
                    _saveSystem.ClearSave();
                    
                    await _loader.InitLoader();
                    
                    await _loader.AnimateProgressTo(0.2f, 1f);
                    
                    await _loader.AnimateProgressTo(0.5f, 1f);
                    
                    _dataLayer.Balances.SetCurrentScore(0);
                    
                    await _scenesService.UnloadLevelScene(_currentLevelType);
                    
                    await _loader.AnimateProgressTo(1f, 1f);
                    
                    await _scenesService.LoadLevelSceneIfNotLoaded(GameLevelType.StartScreen);
                
                
                    await UniTask.Delay(TimeSpan.FromSeconds(1));
                    await _loader.FadeOut();
                    
                    _signalBus.Fire<UnpauseInputSignal>();
                    
                    await UniTask.Delay(500);
                    
                    _signalBus.Fire<UIComponentsInBehaviorSignal>();
                }
            }
        }
    }