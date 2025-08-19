

using Cysharp.Threading.Tasks;

namespace VisualLayer.Loader
{
    public interface ILoader
    {
        #region Methods

        void ResetData();
        
        UniTask FadeIn();
        
        UniTask FadeOut();
        
        void SetProgress(float progress, string text);

        UniTask AnimateProgressTo(float targetProgress, float duration);

        #endregion
    }
}