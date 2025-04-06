using DataLayer.DataTypes;
using UnityEngine;

namespace ServiceLayer.EffectsService
{
    public interface IEffectsManager
    {
        void PlayEffect(EffectType effectType, Vector2 position);
    }
}