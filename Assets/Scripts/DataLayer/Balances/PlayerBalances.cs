using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using ServiceLayer.DataSyncService;
using ServiceLayer.PlayFabService;
using UnityEngine;
using Zenject;

namespace DataLayer.Balances
{
    [Serializable]
    public class PlayerBalances : IPlayerBalances, ISyncableService
    {
        #region Events

        public event Action CoinsBalanceChanged;
        public event Action HighScoreChanged;
        
        public event Action ScoreChanged;
        
        public event Action OnDataChanged;

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

        #region Injects
        
        private IServerService _serverService;
        
        public void Initialize(IServerService serverService)
        {
            _serverService = serverService;
        }

        #endregion
        

        #region Methods
        
        public Dictionary<string, string> GetSyncData()
        {
            return new Dictionary<string, string>
            {
                { "Coins", _coins.ToString() },
                { "HighScore", _highScore.ToString() },
                { "CurrentScore", _currentScore.ToString() }
            };

        }

        public void AddCoins(int coinsToAdd)
        {
            if (coinsToAdd < 0)
            {
                return;
            }
            
            _coins += coinsToAdd;
            CoinsBalanceChanged?.Invoke();
            
            OnDataChanged?.Invoke();
        }

        public bool RemoveCoins(int coinsToRemove)
        {
            if (coinsToRemove <= 0 || coinsToRemove > _coins)
            {
                throw new NotImplementedException();
            }
            
            _coins -= coinsToRemove;
            CoinsBalanceChanged?.Invoke();
            
            OnDataChanged?.Invoke();
            
            return true;
        }

        public void SetHighScore(int newHighScore)
        {
            _highScore = newHighScore;
            HighScoreChanged?.Invoke();
            
            OnDataChanged?.Invoke();
            
        }

        public void AddCurrentScore(int newCurrentScore)
        {
            _currentScore += newCurrentScore;
            ScoreChanged?.Invoke();
            
            OnDataChanged?.Invoke();
        }

        public void SetCurrentScore(int newCurrentScore)
        {
            _currentScore = newCurrentScore;
            ScoreChanged?.Invoke();
            
            OnDataChanged?.Invoke();
        }

        public int GetCurrentScore()
        {
            return _currentScore;
        }

        public int GetCurrentCoins()
        {
            return _coins;
        }
        
        public async UniTask LoadFromServer()
        {
            _coins = 0;
            _highScore = 0;
            _currentScore = 0;
            
            //Debug.Log("Loading player balances");
            var data = await _serverService.GetUserData("Coins", "HighScore", "CurrentScore");

            if (data.TryGetValue("Coins", out var coinsStr) && int.TryParse(coinsStr, out var coins))
            {
                _coins = coins;
                CoinsBalanceChanged?.Invoke();
            }
            
            if (data.TryGetValue("HighScore", out var highStr) && int.TryParse(highStr, out var highScore))
            {
                _highScore = highScore;
                HighScoreChanged?.Invoke();
            }

            if (data.TryGetValue("CurrentScore", out var currentStr) && int.TryParse(currentStr, out var currentScore))
            {
                _currentScore = currentScore;
                ScoreChanged?.Invoke();
            }
            
            //Debug.Log("Finish Loading player balances: "+_coins + " " + _highScore +" CurrentScore: " +_currentScore);
        }
        
        #endregion
    }
}