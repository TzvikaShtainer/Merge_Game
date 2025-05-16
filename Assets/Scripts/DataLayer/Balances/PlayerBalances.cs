using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using ServiceLayer.PlayFabService;
using UnityEngine;
using Zenject;

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

        #region Injects
        
        private IServerService _serverService;
        
        public void Initialize(IServerService serverService)
        {
            _serverService = serverService;
        }

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
            
            _serverService.SetUserData(new Dictionary<string, string>
            {
                { "Coins", _coins.ToString() }
            }).Forget();
        }

        public bool RemoveCoins(int coinsToRemove)
        {
            if (coinsToRemove <= 0 && coinsToRemove > _coins)
            {
                throw new NotImplementedException();
            }
            
            _coins -= coinsToRemove;
            CoinsBalanceChanged?.Invoke();
            
            _serverService.SetUserData(new Dictionary<string, string>
            {
                { "Coins", _coins.ToString() }
            }).Forget();
            
            return true;
        }

        public void SetHighScore(int newHighScore)
        {
            _highScore = newHighScore;
            HighScoreChanged?.Invoke();
            
            _serverService.SetUserData(new Dictionary<string, string>
            {
                { "HighScore", _highScore.ToString() }
            }).Forget();
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
        
        public async UniTask LoadFromServer()
        {
            Debug.Log("Loading player balances");
            var data = await _serverService.GetUserData("Coins", "HighScore");

            if (data.TryGetValue("Coins", out var coinsStr) && int.TryParse(coinsStr, out var coins))
                _coins = coins;

            if (data.TryGetValue("HighScore", out var highStr) && int.TryParse(highStr, out var highScore))
                _highScore = highScore;
            
            Debug.Log("Finish Loading player balances: "+_coins + " " + _highScore);
        }
        
        
        #endregion
        
    }
}