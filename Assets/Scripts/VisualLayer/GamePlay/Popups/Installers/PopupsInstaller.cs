using DataLayer.DataTypes;
using DataLayer.DataTypes.abilities;
using EasyUI.PickerWheelUI;
using ServiceLayer.GameScenes;
using ServiceLayer.NotificationsService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using VisualLayer.GamePlay.Handlers.StartScene;
using VisualLayer.GamePlay.Popups.AboutUs;
using VisualLayer.GamePlay.Popups.InternetConnectionPopup;
using VisualLayer.GamePlay.Popups.MusicMenuPopup;
using VisualLayer.GamePlay.Popups.SpinTheWheelPopup;
using VisualLayer.GamePlay.Popups.YesNoPopup;
using VisualLayer.GamePlay.RewardSystem;
using Zenject;

namespace VisualLayer.GamePlay.Popups.Installers
{
    public class PopupsInstaller : MonoInstaller<PopupsInstaller>
    {
        [Inject]
        private IGameScenesService _scenesService;
        
        [SerializeField] 
        private RectTransform _parentPopupCanvasTransform;
        
        [SerializeField]
        private YesNoPopup.YesNoPopup _yesNoPopupPrefabRef;
        
        [SerializeField]
        private AddSkillsPopup.AddSkillsPopup _addSkillPopupPrefabRef;
        
        [SerializeField]
        private SettingsMenuPopup _settingsMenuPopupPrefabRef;
        
        [SerializeField]
        private InternetConnectionPopup.InternetConnectionPopup _internetConnectionPopup;
        
        [SerializeField]
        private SpinTheWheelPopup.SpinTheWheelPopup  _spinTheWheelPopupPrefabRef;
        
        [SerializeField]
        private SpinTheWheelPopup.SpinWinPopup  _spinWinPopupPrefabRef;
        
        [SerializeField]
        private DailyRewardPopup.DailyRewardPopup  _dailyRewardPopupPrefabRef;
        
        [SerializeField]
        private AboutUsPopup  _aboutUsPopupPrefabRef;
        
        public override void InstallBindings()
        {
            Container
                .BindFactory<AboutUsPopup, AboutUsPopup.Factory>()
                .FromComponentInNewPrefab(_aboutUsPopupPrefabRef)
                .UnderTransform(_parentPopupCanvasTransform)
                .AsSingle();
            
            Container
                .BindFactory<YesNoPopupArgs ,YesNoPopup.YesNoPopup, YesNoPopup.YesNoPopup.Factory>()
                .FromComponentInNewPrefab(_yesNoPopupPrefabRef)
                .UnderTransform(_parentPopupCanvasTransform)
                .AsSingle();
            
            Container
                .BindFactory<AbilityDataSO, AddSkillsPopup.AddSkillsPopup, AddSkillsPopup.AddSkillsPopup.Factory>()
                .FromComponentInNewPrefab(_addSkillPopupPrefabRef)
                .UnderTransform(_parentPopupCanvasTransform)
                .AsSingle();

            Container
                .BindFactory<SettingsMenuPopup, SettingsMenuPopup.Factory>()
                .FromComponentInNewPrefab(_settingsMenuPopupPrefabRef)
                .UnderTransform(_parentPopupCanvasTransform);
            
            Container
                .BindFactory<YesNoPopupArgs, InternetConnectionPopup.InternetConnectionPopup,  InternetConnectionPopup.InternetConnectionPopup.Factory>()
                .FromComponentInNewPrefab(_internetConnectionPopup)
                .UnderTransform(_parentPopupCanvasTransform);
            
            Container
                .BindFactory<SpinTheWheelPopup.SpinTheWheelPopup, SpinTheWheelPopup.SpinTheWheelPopup.Factory>()
                .FromComponentInNewPrefab(_spinTheWheelPopupPrefabRef)
                .UnderTransform(_parentPopupCanvasTransform);

            Container
                .Bind<IRewardSystem>()
                .To<RewardSystem.WheelRewardSystem>()
                .AsSingle();
            
            Container
                .Bind<IWheelHandler>()
                .To<WheelHandler>()
                .AsSingle();
            
            Container
                .BindFactory<Sprite, int, SpinWinPopup, SpinWinPopup.Factory>()
                .FromComponentInNewPrefab(_spinWinPopupPrefabRef)
                .UnderTransform(_parentPopupCanvasTransform);
            
            Container
                .BindFactory<DailyRewardPopup.DailyRewardPopup,  DailyRewardPopup.DailyRewardPopup.Factory>()
                .FromComponentInNewPrefab(_dailyRewardPopupPrefabRef)
                .UnderTransform(_parentPopupCanvasTransform);
            
            Container
                .BindInterfacesAndSelfTo<InternetConnectionService>()
                .AsSingle()
                .NonLazy();

            Container
                .BindInterfacesAndSelfTo<NotificationFlowManager>()
                .AsSingle();
            
            Container
                .Bind<IInitializableNotification>()
                .To<DailyRewardNotification>()
                .AsCached();
            
            Container
                .Bind<IInitializableNotification>()
                .To<SpinTheWheelNotification>()
                .AsCached();
            
        }
        
        private async void Awake()
        {
            Application.targetFrameRate = 60;

        #if !UNITY_EDITOR
            SceneManager.LoadSceneAsync("Loader", LoadSceneMode.Additive);
        #endif
        }
    }
}