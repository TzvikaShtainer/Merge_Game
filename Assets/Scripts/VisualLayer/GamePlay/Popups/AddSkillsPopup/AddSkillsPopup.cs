using Cysharp.Threading.Tasks;
using DataLayer.DataTypes.abilities;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using VisualLayer.GamePlay.Popups.YesNoPopup;
using Zenject;

namespace VisualLayer.GamePlay.Popups.AddSkillsPopup
{
    public class AddSkillsPopup : Popup
    {
        #region Factories

        public class Factory : PlaceholderFactory<AbilityDataSO, AddSkillsPopup> { }

        #endregion
        
        #region Editor
        
        [SerializeField]
        private TextMeshProUGUI _popupSkillName;
        
        [SerializeField]
        private TextMeshProUGUI _popupSkillDescription;
        
        [SerializeField]
        private TextMeshProUGUI _popupSkillCost;
        
        #endregion
        
        #region Fields

        //private UniTaskCompletionSource<> _tcs;

        #endregion

        #region Injects
        
        [Inject]
        public void Construct(AbilityDataSO abilityDataSo)
        {
            _popupSkillName.text = abilityDataSo.Name;
            _popupSkillDescription.text = abilityDataSo.Description;
            _popupSkillCost.text = abilityDataSo.Cost.ToString();
        }
        

        #endregion
        
        
        #region Methods

        

        #endregion
    }
}