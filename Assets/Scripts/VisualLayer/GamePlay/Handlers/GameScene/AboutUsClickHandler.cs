using Cysharp.Threading.Tasks;
using VisualLayer.GamePlay.Popups.AboutUs;
using Zenject;

namespace VisualLayer.GamePlay.Handlers
{
    public class AboutUsClickHandler :IAboutUsClickHandler
    {
        [Inject]
        private AboutUsPopup.Factory _aboutUsPopupFactory;
        
        public async UniTask Execute()
        {
            var popup = _aboutUsPopupFactory.Create();
        }
    }
}