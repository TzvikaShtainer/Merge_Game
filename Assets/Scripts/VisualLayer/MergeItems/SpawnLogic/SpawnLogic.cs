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
        
        private readonly Camera _mainCamera;
        private const float EdgePadding = 0.45f;
        
        [Inject]
        public SpawnLogic(Camera mainCamera)
        {
            _mainCamera = mainCamera;
        }
        public void Spawn(Vector2 posToSpawn)
        {
            Debug.Log("Spawning items");
            _gameLogicHandler.DropCurrentItem(); 
        }

        public void CompleteSpawn(Vector2 posToSpawn)
        {
            _gameLogicHandler.SetNextItem(posToSpawn); 
            _gameLogicHandler.CreateNextItem(); 
        }

        public void UpdateDraggingPosition(Vector2 pos)
        {
            float screenAspect = (float)Screen.width / Screen.height;
            float orthoSize = _mainCamera.orthographicSize;
            
            float maxXBound = (orthoSize * screenAspect) - EdgePadding;
            float minXBound = -maxXBound;
            
            pos.x = Mathf.Clamp(pos.x, minXBound, maxXBound);
            
            _gameLogicHandler.SetCurrItemPosByLocation(pos);
        }
    }
}