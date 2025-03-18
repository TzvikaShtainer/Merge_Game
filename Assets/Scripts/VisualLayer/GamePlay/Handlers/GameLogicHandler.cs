using System;
using UnityEngine;
using VisualLayer.Factories;
using VisualLayer.MergeItems;
using Zenject;
using Random = UnityEngine.Random;

namespace VisualLayer.GamePlay.Handlers
{
    public class GameLogicHandler: IInitializable, IGameLogicHandler
    { 
        private int _min_lvl_spawn = 0;
        private int _max_lvl_spawn = 3;

        [SerializeField]
        private Item _currentItem;
        
        [SerializeField]
        private Item _nextItem;
        
        
        [Inject]
        private ItemFactory _itemFactory;

        public void Initialize()
        {
            CreateFirstItem();
            CreateNextItem();
        }

        private void CreateFirstItem()
        {
            _currentItem = CreateItem();
            
            SetPos(_currentItem);
        }

        private Item CreateItem()
        {
            int randomLvlToSpawn = Random.Range(_min_lvl_spawn, _max_lvl_spawn);
            var itemToSpawn = _itemFactory.Create(randomLvlToSpawn);
            return itemToSpawn;
        }

        private static void SetPos(Item itemToSpawn)
        {
            itemToSpawn.GetComponent<Rigidbody2D>().gravityScale = 0;
            
            // Convert screen center to world position
            Vector3 screenCenter = new Vector3(Screen.width / 2, 2.5f * Screen.height / Camera.main.orthographicSize, Camera.main.nearClipPlane);
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenCenter);

            itemToSpawn.transform.position = new Vector2(worldPosition.x, 2.5f);
        }

        public event Action NextItemCreated;

        public void CreateNextItem()
        {
            _nextItem = CreateItem();
            
            _nextItem.GetComponent<Rigidbody2D>().gravityScale = 0;
            _nextItem.transform.position = new Vector2(10, 10);
            //Debug.Log(_nextItem.name);
            //NextItemCreated?.Invoke();
        }
        
        public Item GeNextItem() => _nextItem;
    }
}