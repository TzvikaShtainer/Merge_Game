using UnityEngine;
using Zenject;

namespace ServiceLayer.SaveSystem
{
    public class SaveSystemGameObject : MonoBehaviour
    {
        [Inject] private ISaveSystem _saveService;
        
        private void OnApplicationPause(bool pause)
        {
            if (pause)
            {
                //Debug.Log("pause");
                _saveService.Save();
            }
        }

        private void OnApplicationQuit()
        {
            //Debug.Log("OnApplicationQuit");
            _saveService.Save();
        }
    }
    
}