using DataLayer.DataTypes;
using UnityEngine;
using Zenject;

namespace ServiceLayer.EffectsService
{
    public class EffectsManager : IEffectsManager
    {
        private readonly EffectsDatabase _effectsDatabase;
        private readonly DiContainer _container;

        public EffectsManager(EffectsDatabase effectsDatabase, DiContainer container)
        {
            _effectsDatabase = effectsDatabase;
            _container = container;
        }

        public void PlayEffect(EffectType effectType, Vector2 position)
        {
            var effect = _effectsDatabase.GetEffect(effectType);
            if (effect == null) return;
            
            var factory = _container.ResolveId<EffectPoolItem.Factory>(effectType);
            var effectInstance = factory.Create();
            effectInstance.transform.position = position;
            
            //Object.Instantiate(effect, position, Quaternion.identity);
            
            //Debug.Log("Effect Played");
        }
    }
}