using Cysharp.Threading.Tasks;
using UnityEngine;
using VisualLayer.Loader;
using Zenject;

namespace VisualLayer.GamePlay.Handlers
{
    public class HudBackClickHandler : IHudBackClickHandler
    {
        [Inject]
        private ILoader _loader;
        
        public async UniTask Execute()
        {
            Debug.Log("sda");
            _loader.ResetData();
            await _loader.FadeIn();
            _loader.SetProgress(0.1f, "Loader");
            await UniTask.Delay(1500);
            _loader.SetProgress(0.2f, "Loader 20%");
            await UniTask.Delay(1500);
            _loader.SetProgress(0.5f, "Loader 50%");
        }
    }
}