using UnityEngine;
using VisualLayer.GamePlay.Popups.YesNoPopup;
using Zenject;

namespace VisualLayer.GamePlay.Popups.Installers
{
    public class PopupsInstaller : MonoInstaller<PopupsInstaller>
    {
        [SerializeField] 
        private RectTransform _parentPopupCanvasTransform;
        
        [SerializeField]
        private YesNoPopup.YesNoPopup _yesNoPopup;
        public override void InstallBindings()
        {
            Container
                .BindFactory<YesNoPopup.YesNoPopup, YesNoPopup.YesNoPopup.Factory>()
                .FromComponentInNewPrefab(_yesNoPopup)
                .UnderTransform(_parentPopupCanvasTransform)
                .AsSingle();
        }
    }
}