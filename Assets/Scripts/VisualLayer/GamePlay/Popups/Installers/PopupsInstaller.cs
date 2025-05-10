using DataLayer.DataTypes.abilities;
using UnityEngine;
using UnityEngine.Serialization;
using VisualLayer.GamePlay.Popups.MusicMenuPopup;
using VisualLayer.GamePlay.Popups.YesNoPopup;
using Zenject;

namespace VisualLayer.GamePlay.Popups.Installers
{
    public class PopupsInstaller : MonoInstaller<PopupsInstaller>
    {
        [SerializeField] 
        private RectTransform _parentPopupCanvasTransform;
        
        [SerializeField]
        private YesNoPopup.YesNoPopup _yesNoPopupPrefabRef;
        
        [SerializeField]
        private AddSkillsPopup.AddSkillsPopup _addSkillPopupPrefabRef;
        
        [SerializeField]
        private SettingsMenuPopup settingsMenuPopupPrefabRef;
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
                .BindFactory<MusicMenuPopup.SettingsMenuPopup, MusicMenuPopup.SettingsMenuPopup.Factory>()
                .FromComponentInNewPrefab(settingsMenuPopupPrefabRef)
                .UnderTransform(_parentPopupCanvasTransform);
        }
        
        
    }
}