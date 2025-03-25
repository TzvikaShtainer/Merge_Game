using ServiceLayer.Signals.SignalsClasses;
using UnityEngine;
using Zenject;

namespace VisualLayer.GamePlay.UI
{
    public class GameplayUiCanvas : MonoBehaviour
    {
        [SerializeField] 
        private GameObject _joystick;
        
        [Inject]
        private SignalBus _signalBus;
        
        public void Awake()
        {
            _signalBus.Subscribe<PauseInput>(PauseInputDriven);
            _signalBus.Subscribe<UnpauseInput>(UnpauseInputDriven);
        }

        private void UnpauseInputDriven()
        {
            Debug.Log("UnpauseInputDriven");
            _joystick.SetActive(true);
        }

        private void PauseInputDriven()
        {
            Debug.Log("PauseInputDriven");
            _joystick.SetActive(false);
        }
    }
}