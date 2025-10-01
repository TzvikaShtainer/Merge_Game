using System;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using ServiceLayer.Signals.SignalsClasses;
using UnityEngine;
using Zenject;

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
        [Inject]
        private SignalBus  _signalBus;
        
        [SerializeField] 
        private MonoBehaviour[] _UIComponents;
        
        [SerializeField] 
        private float animationDuration = 0.4f;
        
        private IUIComponentBehavior[] _components;

        private void Awake()
        {
            _components = _UIComponents.OfType<IUIComponentBehavior>().ToArray();
            
            _signalBus.Subscribe<UIComponentsInBehaviorSignal>(UIComponentsInBehavior);
            _signalBus.Subscribe<UIComponentsOutBehaviorSignal>(UIComponentsOutBehavior);
        }

        private async void UIComponentsOutBehavior()
        {
            await UniTask.WhenAll(_components.Select(c => c.AnimateOut(animationDuration)));
        }

        private async void UIComponentsInBehavior()
        {
            await UniTask.WhenAll(_components.Select(c => c.AnimateIn(animationDuration)));
        }

        private async void Start()
        {
            foreach (var component in _components)
            {
                component.PrepareOffscreen();
            }
        }
    }
}