using System;
using VisualLayer.MergeItems;

namespace VisualLayer.GamePlay.Handlers
{
    public interface IGameLogicHandler
    {
        event Action NextItemCreated;
        
        public void CreateNextItem();

        public Item GeNextItem();
    }
}