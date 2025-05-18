using System;
using UnityEngine;
using Zenject;

namespace ServiceLayer.EffectsService
{
    public class EffectPoolItem : MonoBehaviour, IPoolable<IMemoryPool>, IDisposable
    {
        
        public class Factory : PlaceholderFactory<EffectPoolItem> { }
        
        #region Fields

        private IMemoryPool _pool;
        private ParticleSystem _effectParticle;

        #endregion
        
        #region Methods

        private void Awake()
        {
            _effectParticle = GetComponent<ParticleSystem>();
        }

        public void OnSpawned(IMemoryPool pool)
        {
            _pool = pool;
            gameObject.SetActive(true);
            PlayAndReturnToPool();
            
        }
        public void OnDespawned()
        {
            gameObject.SetActive(false);
        }
        
        public void Dispose()
        {
            _pool?.Despawn(this);
        }

        private void PlayAndReturnToPool()
        {
            if (_effectParticle == null) return;
            
            _effectParticle.Play();
            float totalDuration  = _effectParticle.main.duration + _effectParticle.main.startLifetime.constant;
            Invoke(nameof(ReturnToPool), totalDuration);
        }

        private void ReturnToPool()
        {
            Dispose();
        }
        
        #endregion
        
    }
}