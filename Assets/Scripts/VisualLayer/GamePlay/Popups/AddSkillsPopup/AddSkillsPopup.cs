using Cysharp.Threading.Tasks;
using DataLayer;
using DataLayer.DataTypes.abilities;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
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

        [SerializeField]
        private Button _yesButtonGameObject;
        
        [SerializeField]
        private Button _noButtonGameObject;

        [SerializeField] 
        private Transform _popupSkillSpriteTransform;
        
        private AbilityDataSO _abilityData;
        
        #endregion
        
        #region Fields

        private UniTaskCompletionSource<AddSkillPopupResult> _tcs;

        #endregion

        #region Injects
        
        [Inject]
        private IDataLayer _dataLayer;
        
        [Inject]
        public void Construct(AbilityDataSO abilityDataSo)
        {
            _abilityData = abilityDataSo;
            
            _popupSkillName.text = abilityDataSo.Name;
            _popupSkillDescription.text = abilityDataSo.Description;
            _popupSkillCost.text = "Cost:\n" + abilityDataSo.Cost + " Coins";
            _yesButtonGameObject.interactable = IsHaveMoney(abilityDataSo);
            _noButtonGameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(300, 200); //הגדרה באדיטור לא עבדה לא יודע למה אז מגדיר ידנית
            _popupSkillSpriteTransform.gameObject.GetComponent<Image>().sprite = abilityDataSo.SkillSprite;
        }

        private bool IsHaveMoney(AbilityDataSO abilityDataSo)
        {
            return abilityDataSo.Cost < _dataLayer.Balances.Coins;
        }

        #endregion
        
        
        #region Methods

        public UniTask<AddSkillPopupResult> WaitForResult()
        {
            _tcs = new UniTaskCompletionSource<AddSkillPopupResult>();
            return _tcs.Task;
        }
        public void OnYesButtonClicked()
        {
            var interactionResult = new AddSkillPopupResult {IsCanceled = false};
            _tcs.TrySetResult(interactionResult);
            Close();
        }
        
        public void OnNoButtonClicked()
        {
            var interactionResult = new AddSkillPopupResult {IsCanceled = true};
            _tcs.TrySetResult(interactionResult);
            Close();
        }

        #endregion
    }
}