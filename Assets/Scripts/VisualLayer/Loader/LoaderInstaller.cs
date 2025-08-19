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
using ServiceLayer.Utilis;
using Unity.VisualScripting;
using UnityEngine;
using VisualLayer.GamePlay.Abilities;
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
            Application.targetFrameRate = 60;
            
            await LoadGameScene();
        }

        private async Task LoadGameScene()
        {
            _loader.ResetData();
            await _loader.FadeIn();
            
            await UniTask.Delay(500);
            _loader.SetProgress(0.2f, "Loading Level 20%");
            
            if (await LoginHandler()) return;

            await UniTask.Delay(500);
            await _scenesService.LoadInfraSceneIfNotLoaded(InfraScreenType.GamePopups);
            
            await UniTask.Delay(500);
            _loader.SetProgress(0.5f, "Loading Level 50%");
            
            await _scenesService.LoadLevelSceneIfNotLoaded(GameLevelType.GamePlay);
            
            await UniTask.DelayFrame(100);
            _loader.SetProgress(0.7f, "Loading Level 70%");
            
            await _gameStartupCoordinator.LoadAllDataFromServer();
            
            await UniTask.Delay(1000);
            
            _loader.SetProgress(1f, "Loading Level 100%");
            
            _loader.FadeOut();
            
            _signalBus.Fire<UnpauseInputSignal>();
            //Debug.Log("Fire UnpauseInputSignal");
        }

        private async Task<bool> LoginHandler()
        {
            const int maxRetries = 3;
            const int delayBetweenRetriesMs = 1500;

            bool isLoggedIn = false;
            for (int attempt = 1; attempt <= maxRetries; attempt++)
            {
                isLoggedIn = await _serverService.Login();
                if (isLoggedIn)
                {
                    Debug.Log($"✅ Login succeeded on attempt {attempt}");
                    break;
                }

                Debug.LogWarning($"❌ Login failed. Retrying ({attempt}/{maxRetries})...");
                await UniTask.Delay(delayBetweenRetriesMs);
            }

            if (!isLoggedIn)
            {
                Debug.LogError("🚫 Failed to login after multiple attempts.");
                //Create UI For Faild Login
                return true;
            }

            return false;
        }
    }
}