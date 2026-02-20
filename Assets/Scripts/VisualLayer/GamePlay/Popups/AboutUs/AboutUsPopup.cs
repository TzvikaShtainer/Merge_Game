using ServiceLayer.NavigationService;
using UnityEngine;
using UnityEngine.UI;
using VisualLayer.GamePlay.Popups.MusicMenuPopup;
using Zenject;

namespace VisualLayer.GamePlay.Popups.AboutUs
{
    public class AboutUsPopup : Popup
    {
        [Inject]
        private INavigationService _navigationService;
        
        #region Factories

        public class Factory : PlaceholderFactory<AboutUsPopup>
        {
        }

        #endregion
        
        [SerializeField] 
        private Button _buttonMaagan;
        
        [SerializeField] 
        private Button _buttonTzvika;
        
        [SerializeField] 
        private Button _buttonRan;
        

        [SerializeField] 
        private string _maaganUrl;
        
        [SerializeField] 
        private string _tzvikaUrl;
        
        [SerializeField] 
        private string _ranUrl;


        protected override void Awake()
        {
            base.Awake();
            
            _buttonMaagan.onClick.AddListener(OnMaaganClick);
            _buttonTzvika.onClick.AddListener(OnTzvikaClick);
            _buttonRan.onClick.AddListener(OnranClick);
        }

        public void OnCloseBtnClick() => Close();
        
        public void OnMaaganClick()
        {
            _navigationService.OpenUrl(_maaganUrl);
        }
        
        public void OnTzvikaClick()
        {
            _navigationService.OpenUrl(_tzvikaUrl);
        }
        
        public void OnranClick()
        {
            _navigationService.OpenUrl(_ranUrl);
        }
    }
}