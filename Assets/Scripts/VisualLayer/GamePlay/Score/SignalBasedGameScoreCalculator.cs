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
            _signalBus.Subscribe<AddCoinsSignal>(AddCoinsSignalHandler);
        }
        
        private void ItemMergedSignalHandler()
        {
            _dataLayer.Balances.AddCurrentScore(_params.ItemsMerged); //multi by lvl merge mybe?

            if ( _dataLayer.Balances.CurrentScore > _dataLayer.Balances.HighScore)
            {
                _dataLayer.Balances.SetHighScore(_dataLayer.Balances.CurrentScore); 
            }
        }
        
        private void AddCoinsSignalHandler()
        {
            _dataLayer.Balances.AddCoins(10);
        }
    }
}