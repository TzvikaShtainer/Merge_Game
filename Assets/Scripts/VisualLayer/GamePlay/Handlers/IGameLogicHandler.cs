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

        public void MoveToNextItemLogic(Vector2 posOfClick);
    }
}