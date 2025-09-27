using UnityEngine;
using VisualLayer.GamePlay.Handlers;
using Zenject;

namespace VisualLayer.GamePlay.Buttons
{
    public class EnterLevelButton : MonoBehaviour
    {
        [Inject]
        private IStartGameClickHandler _enterLevelHandler;
        
        
        public void OnClick()
        {
            _enterLevelHandler.Execute();
        }
    }
}