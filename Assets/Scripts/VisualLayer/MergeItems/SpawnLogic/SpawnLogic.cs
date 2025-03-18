using UnityEngine;
using VisualLayer.Factories;
using Zenject;

namespace VisualLayer.MergeItems.SpawnLogic
{
    public class SpawnLogic : ISpawn
    {
        [Inject]
        private ItemFactory _itemFactory;
        
        private float _lastTimeFire;
        
        private int _min_lvl_spawn = 0;
        private int _max_lvl_spawn = 3;
        
        public void Spawn(Vector2 posToSpawn)
        {
            //need to handle spawn on firs click
            
            var isInDelay = Time.time - _lastTimeFire < 0.5; //0.5 just for now
            if (isInDelay)
            {
                return;
            }
            
            Debug.Log("click");
            //int randomLvlToSpawn = Random.Range(_min_lvl_spawn, _max_lvl_spawn);
            //var itemToSpawn = _itemFactory.Create(randomLvlToSpawn);
            //itemToSpawn.transform.position = posToSpawn;
            
            _lastTimeFire = Time.time;
        }
    }
}