using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

namespace VisualLayer.GamePlay.Buttons
{
    public class UIComponent : MonoBehaviour, IUIComponentBehavior
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private EnterDirectionEnum enterDirectionEnum;
        [SerializeField] private float offset = 500f;
        
        private Vector2 _originalPosition;
        
        private void Awake()
        {
            if (_rectTransform == null)
                _rectTransform = GetComponent<RectTransform>();
            
            _originalPosition = _rectTransform.anchoredPosition;
        }
        
        public void PrepareOffscreen()
        {
            Vector2 offscreenPos = _originalPosition;

            switch (enterDirectionEnum)
            {
                case EnterDirectionEnum.FromLeft:
                    offscreenPos.x -= offset;
                    break;
                case EnterDirectionEnum.FromRight:
                    offscreenPos.x += offset;
                    break;
                case EnterDirectionEnum.FromTop:
                    offscreenPos.y += offset;
                    break;
                case EnterDirectionEnum.FromBottom:
                    offscreenPos.y -= offset;
                    break;
            }

            _rectTransform.anchoredPosition = offscreenPos;
        }
        
        public async UniTask AnimateIn(float duration)
        {
            await _rectTransform.DOAnchorPos(_originalPosition, duration)
                .SetEase(Ease.OutBack)
                .AsyncWaitForCompletion();
        }

        public async UniTask AnimateOut(float duration)
        {
            Vector2 offscreenPos = _originalPosition;

            switch (enterDirectionEnum)
            {
                case EnterDirectionEnum.FromLeft:
                    offscreenPos.x -= offset;
                    break;
                case EnterDirectionEnum.FromRight:
                    offscreenPos.x += offset;
                    break;
                case EnterDirectionEnum.FromTop:
                    offscreenPos.y += offset;
                    break;
                case EnterDirectionEnum.FromBottom:
                    offscreenPos.y -= offset;
                    break;
            }

            await _rectTransform.DOAnchorPos(offscreenPos, duration)
                .SetEase(Ease.InBack)
                .AsyncWaitForCompletion();
        }
    }
}