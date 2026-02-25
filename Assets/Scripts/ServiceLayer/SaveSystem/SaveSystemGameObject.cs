using DataLayer;
using UnityEngine;
using Zenject;

namespace ServiceLayer.SaveSystem
{
    public class SaveSystemGameObject : MonoBehaviour
    {
        [Inject] 
        private ISaveSystem _saveService;
        
        [Inject]
        private IDataLayer _dataLayer;
        
        private void OnApplicationPause(bool pause)
        {
            if (pause)
            {
                //Debug.Log("pause - saving now");
                _saveService.Save();
                _dataLayer.Balances.SetCurrentScore(_dataLayer.Balances.CurrentScore);

            }
        }

        private void OnApplicationQuit()
        {
            //Debug.Log("OnApplicationQuit - saving now");
            _saveService.Save();
            _dataLayer.Balances.SetCurrentScore(_dataLayer.Balances.CurrentScore);

        }
    }
    
}