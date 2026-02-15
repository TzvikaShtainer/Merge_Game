using Blast.ServiceLayer.GameScenes;
using ServiceLayer.EffectsService;
using ServiceLayer.GameScenes;
using ServiceLayer.HourlyCoinsService;
using ServiceLayer.MusicService;
using ServiceLayer.NavigationService;
using ServiceLayer.NotificationsService;
using ServiceLayer.PlayFabService;
using ServiceLayer.SaveSystem;
using ServiceLayer.SettingsService;
using ServiceLayer.SpinTheWheelCooldownService;
using ServiceLayer.TimeControl;
using ServiceLayer.TImeProvider;
using ServiceLayer.Utilis;
using UnityEngine;
using VisualLayer.GamePlay.Popups.DailyRewardPopup;
using Zenject;

namespace ServiceLayer.Installers
{
    [CreateAssetMenu(menuName = "Merge/Data/Service Layer Installer", fileName = "Service Layer Installer")]
    public class ServiceLayerInstaller : ScriptableObjectInstaller<ServiceLayerInstaller>
    {
        public override void InstallBindings()
        {
            Container
                .Bind<IGameScenesService>()
                .To<GameScenesService>()
                .AsSingle();
            
            Container
                .Bind<ITimeController>()
                .To<TimeController>()
                .AsSingle();
            
            Container
                .Bind<IEffectsManager>()
                .To<EffectsManager>()
                .AsSingle();

            Container
                .Bind<IServerService>()
                .To<PlayFabService.PlayFabService>()
                .AsSingle();

            Container
                .Bind<ISaveSystem>()
                .To<SaveSystem.SaveSystem>()
                .AsSingle();
            
            Container.BindInterfacesAndSelfTo<GameSettingsService>().AsSingle();
            
            Container
                .Bind<IMusicService>()
                .To<FMODMusicService>()
                .AsSingle();
            
            Container
                .Bind<ISfxService>()
                .To<FMODSfxService>()
                .AsSingle();
            
            Container
                .Bind<ITimeProviderService>()
                .To<TimeProviderService>()
                .AsSingle();
            
            Container
                .Bind<INavigationService>()
                .To<NavigationService.NavigationService>()
                .AsSingle();
            
            
            Container
                .BindInterfacesAndSelfTo<PlayFabHourlyCoinsService>()
                .AsSingle();
    
            Container
                .BindInterfacesAndSelfTo<PlayFabSpinTheWheelCooldownService>()
                .AsSingle();
    
            Container
                .BindInterfacesAndSelfTo<PlayFabDailyRewardCooldownService>()
                .AsSingle();
            
            //---Zenject Dont Need To Bind An Abstract Class
            /*Container
                .Bind<ICooldownService>()
                .To<BaseCooldownService>()
                .AsSingle();*/
            
            Container
                .BindInterfacesAndSelfTo<DataSyncService.DataSyncService>()
                .AsSingle()
                .NonLazy();
            
            Container
                .Bind<GameStartupCoordinator>()
                .AsSingle();
        }
    }
}