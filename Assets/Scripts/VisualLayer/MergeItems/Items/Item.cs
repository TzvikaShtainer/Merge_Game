using UnityEngine;
using Zenject;

namespace VisualLayer.MergeItems
{
    public class Item : MonoBehaviour
    {
        #region Factories

        public class Factory : PlaceholderFactory<Item>
        {
        }
		
        #endregion
    }
}