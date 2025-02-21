using UnityEngine;
using Zenject;

namespace VisualLayer.MergeItems.SpawnLogic
{
    public class SpawnLogic : ISpawn
    {
        [Inject]
        private Item.Factory _itemFactory;
        
        private float _lastTimeFire;
        
        public void Spawn()
        {
            //mybe add timer fow spawn
            var isInDelay = Time.time - _lastTimeFire < 0.5; //0.5 just for now
            if (isInDelay)
            {
                return;
            }
            
            
            var itemToSpawn = _itemFactory.Create();
            
            _lastTimeFire = Time.time;
        }
    }
}