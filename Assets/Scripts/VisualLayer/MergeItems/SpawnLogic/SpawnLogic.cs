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
            _gameLogicHandler.SetCurrItemPosByLocation(pos);
        }
    }
}