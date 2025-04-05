using UnityEngine;

namespace VisualLayer.MergeItems.SpawnLogic
{
    public interface ISpawn
    {
        void Spawn(Vector2 posToSpawn);
        void UpdateDraggingPosition(Vector2 pos);
        public void CompleteSpawn(Vector2 posToSpawn);
        
    }
}