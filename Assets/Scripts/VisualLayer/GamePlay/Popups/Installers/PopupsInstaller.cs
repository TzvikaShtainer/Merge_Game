using DataLayer.DataTypes;
using DataLayer.DataTypes.abilities;
using ServiceLayer.GameScenes;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using VisualLayer.GamePlay.Popups.MusicMenuPopup;
using VisualLayer.GamePlay.Popups.YesNoPopup;
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
        private SettingsMenuPopup settingsMenuPopupPrefabRef;
        
        [SerializeField]
        private InternetConnectionPopup.InternetConnectionPopup _internetConnectionPopup;
        public override void InstallBindings()
        {
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
                .FromComponentInNewPrefab(settingsMenuPopupPrefabRef)
                .UnderTransform(_parentPopupCanvasTransform);
            
            Container
                .BindFactory<YesNoPopupArgs, InternetConnectionPopup.InternetConnectionPopup,  InternetConnectionPopup.InternetConnectionPopup.Factory>()
                .FromComponentInNewPrefab(_internetConnectionPopup)
                .UnderTransform(_parentPopupCanvasTransform);
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