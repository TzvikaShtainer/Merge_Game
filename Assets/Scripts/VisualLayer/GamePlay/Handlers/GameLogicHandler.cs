using System;
using ServiceLayer.SaveSystem;
using UnityEngine;
using VisualLayer.Factories;
using VisualLayer.MergeItems;
using Zenject;
using Random = UnityEngine.Random;

namespace VisualLayer.GamePlay.Handlers
{
    public class GameLogicHandler: IInitializable, IGameLogicHandler
    { 
        public event Action NextItemCreated;
        
        private int _minLvlSpawn = 0;
        private int _maxLvlSpawn = 3;

        [SerializeField]
        private Item _currentItem;
        
        [SerializeField]
        private Item _nextItem;
        
        
        [Inject]
        private ItemFactory _itemFactory;

        public Item GetNextItem() => _nextItem;
        

        public void Initialize()
        {
            HandleFirstItemCreation();
            
            CreateNextItem();
        }

        private void HandleFirstItemCreation()
        {
            //_currentItem = CreateItem();
            
            
            // Convert screen center to world position
            Vector3 screenCenter = new Vector3(Screen.width / 2, 2.5f * Screen.height / Camera.main.orthographicSize, Camera.main.nearClipPlane);
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenCenter);

            _currentItem = CreateItem(new Vector2(worldPosition.x, 2.5f));
            
            SetCurrItemPosByLocation(new Vector2(worldPosition.x, 2.5f));
        }

        private Item CreateItem(Vector2 pos)
        {
            int randomLvlToSpawn = Random.Range(_minLvlSpawn, _maxLvlSpawn);
            var itemToSpawn = _itemFactory.Create(randomLvlToSpawn, pos);
            return itemToSpawn;
        }

        public Item CreateItemFromSave(string typeId, SerializableTypes.SerializableVector2 pos)
        {
            Vector2 position2D = new(pos.x, pos.y);
            int itemId = int.Parse(typeId);
            
            Item newItem = _itemFactory.Create(itemId, position2D);
            
            return newItem;
        }
        
        public void SetCurrItemPosByLocation(Vector2 pos)
        {
            _currentItem.GetComponent<Rigidbody2D>().gravityScale = 0;
            
            _currentItem.transform.position = pos;
        }

        public void CreateNextItem()
        {
            _nextItem = CreateItem(new Vector2(10, 10));
            
            _nextItem.GetComponent<Rigidbody2D>().gravityScale = 0;
            //_nextItem.transform.position = new Vector2(10, 10);

            NextItemCreated?.Invoke();
        }
        
        public void DropCurrentItem()
        {
            _currentItem.GetComponent<Rigidbody2D>().gravityScale = 1;
        }

        public void SetNextItem(Vector2 posOfClick)
        {
            _currentItem = _nextItem;

            posOfClick.x = Mathf.Clamp(posOfClick.x, -1.4f, 1.4f);
            
            SetCurrItemPosByLocation(new Vector2(posOfClick.x, 2.5f));
        }

       
    }
}