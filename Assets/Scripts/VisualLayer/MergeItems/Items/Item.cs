using UnityEngine;
using Zenject;

namespace VisualLayer.MergeItems
{
    public class Item : MonoBehaviour
    {
        private Vector3 _spawnPosition;

        
        #region Factories

        public class Factory : PlaceholderFactory<Vector3, Item>
        {
        }
		
        #endregion

        [Inject]
        private void Construct(Vector3 position)
        {
            _spawnPosition = position;
            transform.position = _spawnPosition;
        }
    }
}