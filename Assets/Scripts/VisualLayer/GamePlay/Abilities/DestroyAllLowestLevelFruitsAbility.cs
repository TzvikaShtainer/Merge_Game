using DataLayer.DataTypes.abilities;
using UnityEngine;
using Zenject;

namespace VisualLayer.GamePlay.Abilities
{
    public class DestroyAllLowestLevelFruitsAbility : IAbility
    {
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
        public void Use()
        {
            if (Count > 0)
            {
                Count--;
               Debug.Log("DestroyAllLowestLevelFruitsAbility has been called.");
            }
        }

        public void Buy()
        {
            throw new System.NotImplementedException();
        }
    }
}