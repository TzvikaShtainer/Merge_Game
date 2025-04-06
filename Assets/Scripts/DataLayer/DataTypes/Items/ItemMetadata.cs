using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace DataLayer.DataTypes
{
    [Serializable]
    public class ItemMetadata
    {
        #region Fields

        [SerializeField] 
        private string _itemName;
        
        [SerializeField] 
        private int _itemId;
        
        [SerializeField]
        private Object _itemPrefabRef;

        [SerializeField]
        private Sprite _itemPreviewSprite;

        #endregion

        
        #region Properties

        public string ItemName => _itemName;
        
        public int ItemId => _itemId;
        
        public Object ItemPrefabRef => _itemPrefabRef;
        
        public Sprite ItemPreviewSprite => _itemPreviewSprite;

        #endregion
    }
}