using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using Zenject;

namespace VisualLayer.GamePlay.Popups.YesNoPopup
{
    public class YesNoPopup : Popup
    {
        #region Factory

        public class Factory : PlaceholderFactory<YesNoPopupArgs, YesNoPopup> { }

        #endregion

        #region Editor

        [SerializeField]
        private TextMeshProUGUI _yesButtonText;
        
        [SerializeField]
        private TextMeshProUGUI _noButtonText;
        
        [SerializeField]
        private GameObject _noButtonGameObject;
        
        [SerializeField]
        private TextMeshProUGUI _popupContent;

        #endregion

        #region Fields

        private UniTaskCompletionSource<YesNoPopupResult> _tcs;

        #endregion

        #region Methods

        [Inject]
        public void Construct(YesNoPopupArgs args)
        {
            _popupContent.text = args.Text;
            _yesButtonText.text = args.YesCaption;
            _noButtonText.text = args.NoCaption;
            _noButtonGameObject.SetActive(args.IsNoButtonVisible);
        }

        public UniTask<YesNoPopupResult> WaitForResult()
        {
            _tcs = new UniTaskCompletionSource<YesNoPopupResult>();

            return _tcs.Task;
        }

        public void OnYesClick()
        {
            var result = new YesNoPopupResult { IsYes = true };
            _tcs.TrySetResult(result);
            
            Close();
        }
        
        public void OnNoClick()
        {
            
            var result = new YesNoPopupResult { IsYes = false };
            _tcs.TrySetResult(result);
            
            Close();
        }
        
        #endregion
    }
}