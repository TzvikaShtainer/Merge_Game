using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using ServiceLayer.PlayFabService;
using UnityEngine;
using VisualLayer.GamePlay.Popups.YesNoPopup;
using Zenject;
using ServiceLayer.Signals.SignalsClasses;
using ServiceLayer.TimeControl;

namespace VisualLayer.GamePlay.Popups.InternetConnectionPopup
{
    public class InternetConnectionService : IInitializable, IDisposable
    {
        [Inject]
        private IServerService _serverService;
        
        [Inject]
        private readonly InternetConnectionPopup.Factory _internetConnectionPopupFactory;
        
        [Inject]
        private readonly SignalBus _signalBus;
        
        [Inject]
        private ITimeController _timeController;
        
        private readonly float _checkIntervalSeconds = 2f;
        private CancellationTokenSource _cts;
        private bool _isHandlingLoss;
        private bool _canMonitor; 
        
        public void Initialize()
        {
            _cts = new CancellationTokenSource();
            
            _signalBus.Subscribe<StartInternetCheckSignal>(OnGameReady);
            
            MonitorConnectionLoop(_cts.Token).Forget();
        }

        private void OnGameReady()
        {
            _canMonitor = true;
        }

        public void Dispose()
        {
            _signalBus.TryUnsubscribe<StartInternetCheckSignal>(OnGameReady);
            _cts?.Cancel();
            _cts?.Dispose();
        }
        
        private async UniTaskVoid MonitorConnectionLoop(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(_checkIntervalSeconds), cancellationToken: token);

                if (token.IsCancellationRequested)
                    break;
                
                if (!_canMonitor)
                    continue;
                
                if (_isHandlingLoss)
                    continue;

                bool isOnline = await CheckInternetConnection(token);

                if (!isOnline)
                {
                    _isHandlingLoss = true;
                    await HandleConnectionLost(token);
                    _isHandlingLoss = false;
                }
            }
        }
        
        private UniTask<bool> CheckInternetConnection(CancellationToken token)
        {
            bool hasInternet = Application.internetReachability != NetworkReachability.NotReachable;
            return UniTask.FromResult(hasInternet);
        }
        
        private async UniTask HandleConnectionLost(CancellationToken token)
        {
            try
            {
                bool reconnected = false;

                while (!reconnected && !token.IsCancellationRequested)
                {
                    var popupArgs = new YesNoPopupArgs
                    {
                        Text = "No Internet Connection",
                        YesCaption = "Try Again",
                        NoCaption = "Exit Game",
                        IsNoButtonVisible = true,
                    };

                    var popup = _internetConnectionPopupFactory.Create(popupArgs);
                    
                    await UniTask.Delay(TimeSpan.FromSeconds(0.2f));
                    
                    _timeController.PauseGameplay();
                    
                    var result = await popup.WaitForResult();
                    
                    _timeController.UnpauseGameplay();

                    if (result.IsYes)
                    {
                        bool hasInternet = await CheckInternetConnection(token);
                        if (!hasInternet)
                            continue;

                        reconnected = await _serverService.Login();

                    }
                    else
                    {
                        Application.Quit();
                    }
                }
            }
            catch
            {
                Debug.Log("Internet connection lost");
            }
        }
    }
}
