using System.Collections.Generic;
using DataLayer.DataTypes;
using UnityEngine;

namespace ServiceLayer.EffectsService
{
    [CreateAssetMenu(menuName = "Merge/Data/Effects Database", fileName = "EffectsDatabase")]
    public class EffectsDatabase : ScriptableObject
    {
        [System.Serializable]
        public class EffectEntry
        {
            public EffectType effectType;
            public ParticleSystem particleSystem;
        }
        
        public List<EffectEntry> effects;

        private Dictionary<EffectType, ParticleSystem> _lookup;
        
        public ParticleSystem GetEffect(EffectType type)
        {
            if (_lookup == null)
            {
                _lookup = new Dictionary<EffectType, ParticleSystem>();
                foreach (var entry in effects)
                {
                    if (!_lookup.ContainsKey(entry.effectType))
                        _lookup.Add(entry.effectType, entry.particleSystem);
                }
            }

            _lookup.TryGetValue(type, out var effect);
            return effect;
        }
    }
}