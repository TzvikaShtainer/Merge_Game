using Cysharp.Threading.Tasks;
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
        
        private UniTaskCompletionSource _closeTaskSource;

        public UniTask WaitForClose()
        {
            _closeTaskSource = new UniTaskCompletionSource();
            return _closeTaskSource.Task;
        }
        
        protected virtual void Close()
        {
            _closeTaskSource?.TrySetResult();
            
            transform.DOScale(Vector3.zero, 0.25f)
                .SetEase(Ease.InBack)
                .OnComplete(() => Destroy(gameObject));
            
            _sfxService.PlaySfxType(SfxType.ClosePopup);
        }
        
        protected virtual void Awake()
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