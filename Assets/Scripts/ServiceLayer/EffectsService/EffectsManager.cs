using DataLayer.DataTypes;
using UnityEngine;

namespace ServiceLayer.EffectsService
{
    public class EffectsManager : IEffectsManager
    {
        private readonly EffectsDatabase _effectsDatabase;

        public EffectsManager(EffectsDatabase effectsDatabase)
        {
            _effectsDatabase = effectsDatabase;
        }

        public void PlayEffect(EffectType effectType, Vector2 position)
        {
            var effect = _effectsDatabase.GetEffect(effectType);
            if (effect == null) return;

            Object.Instantiate(effect, position, Quaternion.identity);
            
            //Debug.Log("Effect Played");
        }
    }
}