using UnityEngine;
using VisualLayer.Factories;
using VisualLayer.MergeItems;
using Zenject;

namespace VisualLayer.GamePlay.Handlers
{
    public class ItemsGeneratorLogic : IItemsGeneratorLogic
    {
        [Inject]
        private ItemFactory _itemFactory;
        
        private int _minLvlSpawn = 0;
        private int _maxLvlSpawn = 3;
        private int _lastGeneratedId = 0;
        private int _counter;
        public int GetRandomItemID()
        {
            int generatedId = Random.Range(_minLvlSpawn, _maxLvlSpawn);

            if (_lastGeneratedId == generatedId)
            {
                _counter++;

                if (_counter >= 2)
                {
                    while (generatedId == _lastGeneratedId)
                    {
                        generatedId = Random.Range(_minLvlSpawn, _maxLvlSpawn);
                    }
                    
                    _counter = 1;
                }
            }
            else
            {
                _counter = 1;
            }
            
            _lastGeneratedId = generatedId;
            return generatedId;
        }
    }
}