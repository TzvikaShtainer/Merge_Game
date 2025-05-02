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
        
        public void Execute(string abilityId)
        {
            var popup = _addSkillsPopupFactory.Create(_abilityManager.GetAbilitySO(abilityId));
        }
    }
}