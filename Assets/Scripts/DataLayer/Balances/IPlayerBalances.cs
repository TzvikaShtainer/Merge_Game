using System;

namespace DataLayer.Balances
{
    public interface IPlayerBalances
    {
        #region Events

        event Action CoinsBalanceChanged;

        #endregion

        #region Properties

        int Coins{get;}
        

        #endregion

        #region Methods

        void AddCoins(int coinsToAdd);
        
        bool RemoveCoins(int coinsToRemove);

        #endregion
    }
}