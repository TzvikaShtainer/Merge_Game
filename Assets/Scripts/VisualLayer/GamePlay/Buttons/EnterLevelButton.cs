using UnityEngine;
using VisualLayer.GamePlay.Handlers;
using Zenject;

namespace VisualLayer.GamePlay.Buttons
{
    public class EnterLevelButton : MonoBehaviour
    {
        private IStartGameClickHandler _enterLevelHandler;

        
        [Inject]
        public void Construct(IStartGameClickHandler enterLevelHandler)
        {
            _enterLevelHandler = enterLevelHandler;
         }
        
        public void OnClick()
        {
            _enterLevelHandler.Execute();
        }
    }
}