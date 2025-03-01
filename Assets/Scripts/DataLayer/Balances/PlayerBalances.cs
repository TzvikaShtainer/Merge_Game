using System;
using UnityEngine;

namespace DataLayer.Balances
{
    [Serializable]
    public class PlayerBalances : IPlayerBalances
    {
        #region Events

        public event Action CoinsBalanceChanged;

        #endregion
        
        #region Editor

        [SerializeField]
        private int _coins;
        

        #endregion

        #region Properties

        public int Coins => _coins;

        #endregion

        #region Methods

        public void AddCoins(int coinsToAdd)
        {
            if (coinsToAdd < 0)
            {
                return;
            }
            
            _coins += coinsToAdd;
            CoinsBalanceChanged?.Invoke();
        }

        public bool RemoveCoins(int coinsToRemove)
        {
            if (coinsToRemove <= 0 && coinsToRemove > _coins)
            {
                throw new NotImplementedException();
            }
            
            _coins -= coinsToRemove;
            CoinsBalanceChanged?.Invoke();
            return true;
        }

        #endregion
        
    }
}