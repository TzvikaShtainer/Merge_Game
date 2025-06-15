using Cysharp.Threading.Tasks;
using VisualLayer.GamePlay.Handlers;

namespace ServiceLayer.SaveSystem
{
    public interface ISaveSystem
    {
        void Save();
        public UniTask Load();
        void Init(GameLogicHandler gameLogicHandler);
    }
}