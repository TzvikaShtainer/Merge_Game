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
            
            ScheduleSave();
            
            _serverService.SetUserData(new Dictionary<string, string>
            {
                { "Coins", _coins.ToString() }
            }).Forget();
        }

        public bool RemoveCoins(int coinsToRemove)
        {
            if (coinsToRemove <= 0 || coinsToRemove > _coins)
            {
                throw new NotImplementedException();
            }
            
            _coins -= coinsToRemove;
            CoinsBalanceChanged?.Invoke();
            
            ScheduleSave();
            
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
            
            ScheduleSave();
            
            _serverService.SetUserData(new Dictionary<string, string>
            {
                { "HighScore", _highScore.ToString() }
            }).Forget();
        }

        public void AddCurrentScore(int newCurrentScore)
        {
            _currentScore += newCurrentScore;
            ScoreChanged?.Invoke();
            
            ScheduleSave();
            
            _serverService.SetUserData(new Dictionary<string, string>
            {
                { "CurrentScore", _currentScore.ToString() }
            }).Forget();
        }

        public void SetCurrentScore(int newCurrentScore)
        {
            _currentScore = newCurrentScore;
            ScoreChanged?.Invoke();
            
            ScheduleSave();
            
            _serverService.SetUserData(new Dictionary<string, string>
            {
                { "CurrentScore", _currentScore.ToString() }
            }).Forget();
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
        
        private bool _saveScheduled;

        private void ScheduleSave()
        {
            if (_saveScheduled)
                return;

            _saveScheduled = true;
            SaveDebounced().Forget();
        }

        private async UniTaskVoid SaveDebounced()
        {
            await UniTask.Delay(500);
            _saveScheduled = false;

            await _serverService.SetUserData(new Dictionary<string, string>
            {
                { "Coins", _coins.ToString() },
                { "HighScore", _highScore.ToString() },
                { "CurrentScore", _currentScore.ToString() }
            });
        }
        
        #endregion
    }
}