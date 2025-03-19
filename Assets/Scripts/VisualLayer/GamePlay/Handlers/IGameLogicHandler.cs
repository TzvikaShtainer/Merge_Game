using System;
using UnityEngine;
using VisualLayer.MergeItems;

namespace VisualLayer.GamePlay.Handlers
{
    public interface IGameLogicHandler
    {
        event Action NextItemCreated;
        
        public void CreateNextItem();

        public Item GetNextItem();

        public void DropCurrentItem();

        public void SetNextItem(Vector2 posOfClick);

        public void SetCurrItemPosByLocation(Vector2 pos);
    }
}