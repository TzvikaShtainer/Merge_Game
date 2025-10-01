using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace VisualLayer.GamePlay.Buttons
{
    public enum EnterDirectionEnum
    {
        FromLeft,
        FromRight,
        FromTop,
        FromBottom
    }
    public class StartScreenComponentsBehaviorManager : MonoBehaviour
    {
        [SerializeField] 
        private MonoBehaviour[] _UIComponents;
        
        [SerializeField] 
        private float animationDuration = 0.4f;
        
        private IUIComponentBehavior[] _components;

        private void Awake()
        {
            _components = _UIComponents.OfType<IUIComponentBehavior>().ToArray();
        }

        private async void Start()
        {
            foreach (var component in _components)
            {
                component.PrepareOffscreen();
            }
            
            await UniTask.Delay(7000); //change to signal
            
            await UniTask.WhenAll(_components.Select(c => c.AnimateIn(animationDuration)));
            
            //For Animation Out
            //await UniTask.WhenAll(_components.Select(c => c.AnimateOut(animationDuration)));

        }
    }
}