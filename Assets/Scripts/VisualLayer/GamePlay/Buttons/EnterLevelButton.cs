using UnityEngine;
using VisualLayer.GamePlay.Handlers;
using Zenject;

namespace VisualLayer.GamePlay.Buttons
{
    public class EnterLevelButton : UIButtonFeedback
    {
        [Inject]
        private IStartGameClickHandler _enterLevelHandler;
        
        
        public void OnClick()
        {
            base.OnClick();
            _enterLevelHandler.Execute();
        }
    }
}