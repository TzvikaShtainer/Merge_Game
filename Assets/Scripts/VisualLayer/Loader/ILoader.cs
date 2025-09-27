

using System.Threading.Tasks;
using Cysharp.Threading.Tasks;

namespace VisualLayer.Loader
{
    public interface ILoader
    {
        #region Methods

        UniTask InitLoader();
        void ResetData();
        
        UniTask FadeIn();
        
        UniTask FadeOut();
        
        void SetProgress(float progress, string text);

        UniTask AnimateProgressTo(float targetProgress, float duration);

        #endregion
    }
}