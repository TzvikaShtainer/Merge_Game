using UnityEngine;
using VisualLayer.Factories;
using VisualLayer.GamePlay.Handlers;
using Zenject;

namespace VisualLayer.MergeItems.SpawnLogic
{
    public class SpawnLogic : ISpawn
    {
        [Inject]
        private ItemFactory _itemFactory;
        
        [Inject]
        private IGameLogicHandler _gameLogicHandler;
        
        private float _lastTimeFire;
        
        private int _min_lvl_spawn = 0;
        private int _max_lvl_spawn = 3;
        private bool _firstClickCounter = true;
        
        public void Spawn(Vector2 posToSpawn)
        {
            var isInDelay = Time.time - _lastTimeFire < 0.5; //0.5 just for now
            //Debug.Log(_firstClickCounter);
            
            if (_firstClickCounter)
            {
               // Debug.Log("_firstClickCounter");
                _firstClickCounter = false;
                _gameLogicHandler.SetCurrItemPosByLocation(posToSpawn);
                _gameLogicHandler.DropCurrentItem();
                return;
            }
            
            if (isInDelay)
            {
               // Debug.Log("delay");
                return;
            }
            
           // Debug.Log("Set And Drop");
            _gameLogicHandler.SetNextItem(posToSpawn);
            
            _gameLogicHandler.DropCurrentItem();
            
            
            
            
            
            
            
            
            _lastTimeFire = Time.time;
        }
    }
}