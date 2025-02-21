using UnityEngine;
using Zenject;

namespace VisualLayer.MergeItems.SpawnLogic
{
    public class SpawnLogic : ISpawn
    {
        [Inject]
        private Item.Factory _itemFactory;
        
        
        public void Spawn()
        {
            //mybe add timer fow spawn
            
            
            var itemToSpawn = _itemFactory.Create();
        }
    }
}