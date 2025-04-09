using UnityEngine;

namespace DataLayer.DataTypes.abilities
{
    [CreateAssetMenu(fileName = "AbilityDataSO", menuName = "Abilities/Ability Data")]
        
    public class AbilityDataSO : ScriptableObject
    {
        [SerializeField] private string _id;
        [SerializeField] private int _count;
        
        public string Id => _id;
    
        public int Count
        {
            get => _count;
            set => _count = Mathf.Max(0, value); //שלא ירד מתחת ל0
        }
    }
}