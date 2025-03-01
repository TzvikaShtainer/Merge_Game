

using Cysharp.Threading.Tasks;

namespace VisualLayer.Loader
{
    public interface ILoader
    {
        #region Methods

        void ResetData();
        
        UniTask FadeIn();

        #endregion
    }
}