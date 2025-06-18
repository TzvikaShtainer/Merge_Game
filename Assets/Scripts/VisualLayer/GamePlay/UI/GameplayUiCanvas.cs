using ServiceLayer.Signals.SignalsClasses;
using UnityEngine;
using Zenject;

namespace VisualLayer.GamePlay.UI
{
    public class GameplayUiCanvas : MonoBehaviour
    {
        [SerializeField] 
        private GameObject _joystick;
        
        [SerializeField]
        private GameObject _gameplayCanvas;
        
        [Inject]
        private SignalBus _signalBus;
        
        public void Awake()
        {
            _signalBus.Subscribe<PauseInputSignal>(PauseInputDriven);
            _signalBus.Subscribe<UnpauseInputSignal>(UnpauseInputDriven);
            
            _signalBus.Subscribe<EnableUISignal>(EnableUICanvas);
            _signalBus.Subscribe<DisableUISignal>(DisableUICanvas);
        }

        private void UnpauseInputDriven()
        {
            _joystick.SetActive(true);
        }

        private void PauseInputDriven()
        {
            _joystick.SetActive(false);
        }

        private void EnableUICanvas()
        {
            _gameplayCanvas.SetActive(true);
        }
        
        private void DisableUICanvas()
        {
            _gameplayCanvas.SetActive(false);
        }
    }
}