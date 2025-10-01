using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DataLayer;
using DataLayer.DataTypes;
using ServiceLayer.GameScenes;
using ServiceLayer.PlayFabService;
using ServiceLayer.SaveSystem;
using ServiceLayer.SettingsService;
using ServiceLayer.Signals.SignalsClasses;
using ServiceLayer.TimeControl;
using ServiceLayer.Utilis;
using Unity.VisualScripting;
using UnityEngine;
using VisualLayer.GamePlay.Abilities;
using VisualLayer.GamePlay.Popups.InternetConnectionPopup;
using VisualLayer.GamePlay.Popups.YesNoPopup;
using Zenject;

namespace VisualLayer.Loader
{
    public class LoaderInstaller : MonoInstaller<LoaderInstaller>
    {
        [Inject]
        private IGameScenesService _scenesService;
        
        [Inject]
        private IServerService _serverService;
        
        [Inject]
        private GameStartupCoordinator  _gameStartupCoordinator;
        
        [Inject]
        private SignalBus _signalBus;
        
        [Inject]
        private InternetConnectionPopup.Factory _internetConnectionPopupFactory;
        
        [Inject]
        private ITimeController _timeController;
        
        #region Loader

        [SerializeField]
        private Loader _loader;

        #endregion
        
        public override void InstallBindings()
        {
            Container
                .Bind<ILoader>()
                .FromInstance(_loader)
                .AsSingle();
        }

        private async void Awake()
        {
            await LoadScene();
        }

        private async Task LoadScene()
        {
            await _loader.InitLoader();
            
            await UniTask.Delay(500);
            await _loader.AnimateProgressTo(0.2f, 0.5f);
            
            await UniTask.Delay(500);
            await _scenesService.LoadInfraSceneIfNotLoaded(InfraScreenType.GamePopups);
            
            await _loader.AnimateProgressTo(0.35f, 0.5f);
            
            if (await TryLoginToGame())
            {
                await LoadGame();
            }
            else
            {
                HandleFailedToLoginGame();
            }
        }

        private async void HandleFailedToLoginGame()
        {
            bool connected = false;

            while (!connected)
            {
                var popupArgs = new YesNoPopupArgs()
                {
                    Text = "Failed To Login To Game",
                    YesCaption = "Try again",
                    NoCaption = "Exit Game",
                    IsNoButtonVisible = true,
                };

                var internetConnectionPopup = _internetConnectionPopupFactory.Create(popupArgs);
                var result = await internetConnectionPopup.WaitForResult();

                if (result.IsYes)
                {
                    connected = await TryLoginToGame();
                    if (connected)
                    {
                        await LoadGame();
                        break;
                    }
                }
                else
                {
                   Application.Quit();
                }
            }
        }
        

        private async Task<bool> TryLoginToGame()
        {
            const int maxRetries = 3; 
            const int delayBetweenRetriesMs = 1500;

            
            for (int attempt = 1; attempt <= maxRetries; attempt++)
            {
                bool isLoggedIn = await _serverService.Login();
                if (isLoggedIn)
                {
                    //Debug.Log($"✅ Login succeeded on attempt {attempt}");
                    return true;
                }

                //Debug.LogWarning($"❌ Login failed. Retrying ({attempt}/{maxRetries})...");
                await UniTask.Delay(delayBetweenRetriesMs);
            }

            //Debug.LogError("🚫 Failed to login after multiple attempts.");
            return false; 
            
        }
        private async Task LoadGame()
        {
            await _loader.AnimateProgressTo(0.5f, 0.5f);
            
            await _scenesService.LoadLevelSceneIfNotLoaded(GameLevelType.StartScreen);
            
            await _loader.AnimateProgressTo(0.85f, 0.5f);
            
            await _gameStartupCoordinator.LoadAllDataFromServer();
            
            await _loader.AnimateProgressTo(1.0f, 0.5f);
            
            await _loader.FadeOut();
            
            _signalBus.Fire<UnpauseInputSignal>();
            
            await UniTask.Delay(500);
            
            _signalBus.Fire<UIComponentsInBehaviorSignal>();
        }
    }
}