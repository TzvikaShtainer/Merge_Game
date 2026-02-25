using Cysharp.Threading.Tasks;
using VisualLayer.GamePlay.Handlers;

namespace ServiceLayer.SaveSystem
{
    public interface ISaveSystem
    {
        UniTask Save();
        public UniTask Load();
        void Init(GameLogicHandler gameLogicHandler);
        public UniTask ClearSave();
    }
}