using System;
using UnityEngine;

namespace DataLayer.Balances
{
    [Serializable]
    public class PlayerBalances : IPlayerBalances
    {
        #region Events

        public event Action CoinsBalanceChanged;
        public event Action HighScoreChanged;
        
        public event Action ScoreChanged;

        #endregion
        
        #region Editor

        [SerializeField]
        private int _coins;
        
        [SerializeField]
        private int _highScore;
        
        [SerializeField]
        private int _currentScore;

        #endregion

        #region Properties

        public int Coins => _coins;
        
        public int HighScore => _highScore;
        public int CurrentScore => _currentScore;

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

        public void SetHighScore(int newHighScore)
        {
            _highScore = newHighScore;
            HighScoreChanged?.Invoke();
        }

        public void AddCurrentScore(int newCurrentScore)
        {
            _currentScore += newCurrentScore;
            ScoreChanged?.Invoke();
        }

        public void SetCurrentScore(int newCurrentScore)
        {
            _currentScore = newCurrentScore;
            ScoreChanged?.Invoke();
        }

        #endregion
        
    }
}