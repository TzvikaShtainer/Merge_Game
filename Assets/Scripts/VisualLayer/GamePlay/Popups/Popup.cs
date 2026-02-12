using DG.Tweening;
using ServiceLayer.MusicService;
using UnityEngine;
using Zenject;

namespace VisualLayer.GamePlay.Popups
{
    public class Popup : MonoBehaviour
    {
        [Inject]
        private ISfxService  _sfxService;
        
        protected virtual void Close()
        {
            transform.DOScale(Vector3.zero, 0.25f)
                .SetEase(Ease.InBack)
                .OnComplete(() => Destroy(gameObject));
            
            _sfxService.PlaySfxType(SfxType.ClosePopup);
        }
        
        private void Awake()
        {
            transform.localScale = Vector3.zero;
            //Todo: Add sfx here?
        }

        protected virtual void OnEnable()
        {
            transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBack);
        }
    }
}