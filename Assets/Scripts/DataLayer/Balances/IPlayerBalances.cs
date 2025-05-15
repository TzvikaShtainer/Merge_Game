using System;
using Cysharp.Threading.Tasks;

namespace DataLayer.Balances
{
    public interface IPlayerBalances
    {
        #region Events

        event Action CoinsBalanceChanged;
        
        event Action HighScoreChanged;
        event Action ScoreChanged;

        #endregion

        #region Properties

        int Coins{get;}
        
        int HighScore{get;}
        
        int CurrentScore{get;}
        

        #endregion

        #region Methods

        void AddCoins(int coinsToAdd);
        
        bool RemoveCoins(int coinsToRemove);
        
        void SetHighScore(int newHighScore);
        
        void AddCurrentScore(int newCurrentScore);
        
        void SetCurrentScore(int newCurrentScore);

        public UniTask LoadFromServer();

        #endregion
    }
}