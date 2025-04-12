using System;
using Cysharp.Threading.Tasks;
using DataLayer.DataTypes.abilities;
using DG.Tweening;
using ServiceLayer.EffectsService;
using ServiceLayer.Signals.SignalsClasses;
using UnityEngine;
using Zenject;

namespace VisualLayer.GamePlay.Abilities
{
    public class ShakeBoxAbility : IAbility
    {
        //Multi Clicks
        [Inject]
        private SignalBus _signalBus;
        
        [Inject] 
        private IEffectsManager _effectsManager;
        
        [Inject(Id = "ShakeBoxJar")]
        private Transform _jarTransform;
        
        [Inject(Id = "MainGameplayCamera")]
        private Camera _mainCamera;
        
        private float _originalOrthoSize;
        
        public string Id => _abilityDataSo.Id;
        public int Count
        {
            get => _abilityDataSo.Count;
            set => _abilityDataSo.Count = value;
        }
        
        private AbilityDataSO _abilityDataSo;

        [Inject]
        public void Construct(AbilityDataSO abilityDataSo)
        {
            _abilityDataSo = abilityDataSo;
        }
        
        public void Buy()
        {
            throw new System.NotImplementedException();
        }
        
        public async void UseAbility()
        {
            if (Count <= 0)
                return;
            
            Count--;
            
            _signalBus.Fire<PauseInput>();
            _signalBus.Fire<DisableUI>();

             ZoomOutFOV();
             await UniTask.Delay(TimeSpan.FromSeconds(0.5));
            
             ShakeBox();
             await UniTask.Delay(TimeSpan.FromSeconds(2.5));
            
            ZoomInFOV();
            await UniTask.Delay(TimeSpan.FromSeconds(0.5));
            
            await UniTask.Delay(TimeSpan.FromSeconds(0.5));
            _signalBus.Fire<UnpauseInput>();
            _signalBus.Fire<EnableUI>();
        }

        private void ShakeBox()
        {
            //cancel previous shake
            _jarTransform.DOKill();

            //save start location
            Vector3 startRotation = _jarTransform.eulerAngles;
            float shakeAngle = 15f; 
            float shakeDuration = 0.3f;

            Sequence shakeSequence = DOTween.Sequence();
            
            //Right shake
            shakeSequence.Append(
                _jarTransform.DORotate(
                    new Vector3(startRotation.x, startRotation.y, startRotation.z + shakeAngle),
                    shakeDuration
                ).SetEase(Ease.InOutSine)
            );
            
            //Middle back
            shakeSequence.Append(
                _jarTransform.DORotate(
                    new Vector3(startRotation.x, startRotation.y, startRotation.z),
                    shakeDuration
                ).SetEase(Ease.InOutSine)
            );
             
            //Left shake
            shakeSequence.Append(
                _jarTransform.DORotate(
                    new Vector3(startRotation.x, startRotation.y, startRotation.z - shakeAngle),
                    shakeDuration
                ).SetEase(Ease.InOutSine)
            );

            //Middle back
            shakeSequence.Append(
                _jarTransform.DORotate(
                    new Vector3(startRotation.x, startRotation.y, startRotation.z),
                    shakeDuration
                ).SetEase(Ease.InOutSine)
            );

            //How Many Times
            shakeSequence.SetLoops(2);

            //Back To Middle
            shakeSequence.OnComplete(() =>
            {
                _jarTransform.rotation = Quaternion.Euler(startRotation);
            });
        }
        
        private void ZoomOutFOV()
        {
            if (_mainCamera == null)
            {
                Debug.LogWarning("Main camera not assigned!");
                return;
            }

            _mainCamera.DOKill();

            if (_mainCamera.orthographic)
            {
                // אם המצלמה אורטוגרפית, נשמור את ה-orthographicSize
                _originalOrthoSize = _mainCamera.orthographicSize;

                float targetOrthoSize = _originalOrthoSize + 2f; // כמה להתרחב
                float zoomDuration = 0.5f;

                DOTween.To(
                    () => _mainCamera.orthographicSize,
                    x => _mainCamera.orthographicSize = x,
                    targetOrthoSize,
                    zoomDuration
                ).SetEase(Ease.InOutSine);
            }
            else
            {
                Debug.LogWarning("Camera is not orthographic, expected orthographic mode.");
            }
        }

        private void ZoomInFOV()
        {
            if (_mainCamera == null)
            {
                Debug.LogWarning("Main camera not assigned!");
                return;
            }

            _mainCamera.DOKill();

            if (_mainCamera.orthographic)
            {
                float zoomDuration = 0.5f;

                DOTween.To(
                    () => _mainCamera.orthographicSize,
                    x => _mainCamera.orthographicSize = x,
                    _originalOrthoSize,
                    zoomDuration
                ).SetEase(Ease.InOutSine);
            }
            else
            {
                Debug.LogWarning("Camera is not orthographic, expected orthographic mode.");
            }
        }
    }
}