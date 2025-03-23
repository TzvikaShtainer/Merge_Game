using DataLayer;
using ServiceLayer.Signals.SignalsClasses;
using UnityEngine;
using Zenject;

namespace VisualLayer.GamePlay.Score
{
    public class SignalBasedGameScoreCalculator : IInitializable
    {
        #region Injects

        [Inject]
        private IDataLayer _dataLayer;
        
        [Inject]
        private SignalBus _signalBus;
        
        [Inject]
        private ScoreCalculationParams _params;

        #endregion
        
        
        public void Initialize()
        {
            _signalBus.Subscribe<ItemMergedSignal>(ItemMergedSignalHandler);
        }

        private void ItemMergedSignalHandler()
        {
            //Debug.Log("ItemMergedSignalHandler");
            _dataLayer.Balances.AddCoins(_params.ItemsMerged); //multi by lvl merge mybe?
        }
    }
}