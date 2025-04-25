using UnityEngine;
using VisualLayer.GamePlay.Handlers;
using Zenject;

namespace VisualLayer.GamePlay.Buttons
{
    public class EnterLevelButton : MonoBehaviour
    {
        [Inject]
        private IStartGameClickHandler _enterLevelHandler;
        
        private void Awake()
        {
            Debug.Log("Start Btn Awake. Handler is " + (_enterLevelHandler != null));
        }
        
        public void OnClick()
        {
            _enterLevelHandler.Execute();
        }
    }
}