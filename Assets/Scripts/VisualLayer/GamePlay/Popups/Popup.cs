using UnityEngine;

namespace VisualLayer.GamePlay.Popups
{
    public class Popup : MonoBehaviour
    {
        protected virtual void Close()
        {
            Destroy(gameObject);
        }
    }
}