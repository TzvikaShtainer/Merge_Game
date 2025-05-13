using DG.Tweening;
using UnityEngine;

namespace VisualLayer.GamePlay.Popups
{
    public class Popup : MonoBehaviour
    {
        protected virtual void Close()
        {
            transform.DOScale(Vector3.zero, 0.25f)
                .SetEase(Ease.InBack)
                .OnComplete(() => Destroy(gameObject));
        }
        
        private void Awake()
        {
            transform.localScale = Vector3.zero;
        }
        
        private void OnEnable()
        {
            transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBack);
        }
    }
}