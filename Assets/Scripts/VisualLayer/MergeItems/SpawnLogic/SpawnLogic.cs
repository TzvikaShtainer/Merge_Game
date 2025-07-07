using UnityEngine;
using VisualLayer.Factories;
using VisualLayer.GamePlay.Handlers;
using Zenject;

namespace VisualLayer.MergeItems.SpawnLogic
{
    public class SpawnLogic : ISpawn
    {
        [Inject]
        private IGameLogicHandler _gameLogicHandler;

        private float _manXValue = 1.7f;
        private float _minXValue = -1.7f;
        public void Spawn(Vector2 posToSpawn)
        {
            _gameLogicHandler.DropCurrentItem(); 
        }

        public void CompleteSpawn(Vector2 posToSpawn)
        {
            _gameLogicHandler.SetNextItem(posToSpawn); 
            _gameLogicHandler.CreateNextItem(); 
        }

        public void UpdateDraggingPosition(Vector2 pos)
        {
            pos.x = Mathf.Clamp(pos.x, _minXValue, _manXValue);
            
            _gameLogicHandler.SetCurrItemPosByLocation(pos);
        }
    }
}