using System;
using UnityEngine;

namespace VisualLayer.GamePlay.Score
{
    [Serializable]
    public class ScoreCalculationParams
    {
        [SerializeField] 
        private int _itemsMerged;
        
        
        public int ItemsMerged => _itemsMerged;
    }
}