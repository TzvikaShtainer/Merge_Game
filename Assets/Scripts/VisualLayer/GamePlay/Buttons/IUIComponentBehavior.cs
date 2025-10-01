using Cysharp.Threading.Tasks;

namespace VisualLayer.GamePlay.Buttons
{
    public interface IUIComponentBehavior
    {
        UniTask AnimateIn(float duration);
        UniTask AnimateOut(float duration);

        void PrepareOffscreen();
    }
}