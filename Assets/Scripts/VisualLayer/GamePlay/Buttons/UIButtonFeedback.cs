using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace VisualLayer.GamePlay.Buttons
{
    public class UIButtonFeedback : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private float scaleUp = 1.1f;
        [SerializeField] private float scaleDuration = 0.1f;

        private Vector3 _originalScale;

        protected  void Awake()
        {
            _originalScale = transform.localScale;
            button.onClick.AddListener(OnClick);
        }

        protected  void OnClick()
        {
            transform.DOScale(scaleUp, scaleDuration).OnComplete(() =>
                transform.DOScale(_originalScale, scaleDuration));
        }
    }
}