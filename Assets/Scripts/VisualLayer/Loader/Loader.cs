using System;
using Cysharp.Threading.Tasks;
using Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace VisualLayer.Loader
{
    public class Loader : MonoBehaviour, ILoader
    {
        #region Editor
        
        [SerializeField]
        private Slider _loaderSlider;
        
        [SerializeField] 
        private TextMeshProUGUI _loaderText;

        [SerializeField] 
        private Animation _animationComponent;
        
        [SerializeField] 
        private AnimationClip _FadeInClip;
        
        [SerializeField]
        private AnimationClip _FadeOutClip;
        
        #endregion
        
        #region Methods

        public void ResetData()
        {
            _loaderSlider.value = 0f;
            _loaderText.text = String.Empty;
        }

        public async UniTask FadeIn()
        {
            await _animationComponent.PlayClipAsync(_FadeInClip);
        }

        public async UniTask FadeOut()
        {
            await _animationComponent.PlayClipAsync(_FadeOutClip);
        }

        public void SetProgress(float progress, string text)
        {
            _loaderSlider.value = progress;
            _loaderText.text = text;
        }

        #endregion
    }
}