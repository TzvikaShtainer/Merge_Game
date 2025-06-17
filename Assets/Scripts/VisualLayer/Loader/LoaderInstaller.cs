using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DataLayer;
using DataLayer.DataTypes;
using ServiceLayer.GameScenes;
using ServiceLayer.PlayFabService;
using ServiceLayer.SaveSystem;
using ServiceLayer.Signals.SignalsClasses;
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
        private IDataLayer _dataLayer;
        
        [Inject]
        private AbilityManager _abilityManager;
        
        [Inject]
        private ISaveSystem _saveSystem;
        
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
            await LoadGameScene();
        }

        private async Task LoadGameScene()
        {
            _loader.ResetData();
            await _loader.FadeIn();
            
            await UniTask.Delay(500);
            _loader.SetProgress(0.2f, "Loading Level 20%");
            
            await _serverService.Login();
            
            await _dataLayer.Balances.LoadFromServer();
            
            await UniTask.Delay(1000);
            await _scenesService.LoadInfraSceneIfNotLoaded(InfraScreenType.GamePopups);
            
            await UniTask.Delay(1000);
            _loader.SetProgress(0.5f, "Loading Level 50%");
            
            await _scenesService.LoadLevelSceneIfNotLoaded(GameLevelType.GamePlay);
            
            await UniTask.DelayFrame(100);
            _loader.SetProgress(0.7f, "Loading Level 70%");
            
            await _abilityManager.LoadFromServer();
            
            await UniTask.Delay(500);
            
            await _saveSystem.Load();
            
            _loader.SetProgress(1f, "Loading Level 100%");
            
            _loader.FadeOut();
        }
    }
}