using ServiceLayer.Signals.SignalsClasses;
using UnityEngine;
using VisualLayer.GamePlay.Abilities;
using VisualLayer.GamePlay.Popups.AddSkillsPopup;
using Zenject;

namespace VisualLayer.GamePlay.Handlers
{
    public class HudPlusCurrencyButtonClickHandler : IHudPlusCurrencyClickHandler
    {
        [Inject]
        AddSkillsPopup.Factory _addSkillsPopupFactory;
        
        [Inject] 
        private AbilityManager _abilityManager;
        
        [Inject]
        private SignalBus _signalBus;
        
        public async void Execute(string abilityId)
        {
            _signalBus.Fire<PauseInput>();
            
            var popup = _addSkillsPopupFactory.Create(_abilityManager.GetAbilitySO(abilityId));

            var popupInteractionResult = await popup.WaitForResult();

            
            if (popupInteractionResult.IsCanceled)
            {
                _signalBus.Fire<UnpauseInput>();
                return;
            }
            
            _abilityManager.BuyAbility(abilityId);
            
            _signalBus.Fire<UnpauseInput>();
        }
    }
}