using UnityEngine;

namespace DataLayer.DataTypes.abilities
{
    [CreateAssetMenu(fileName = "AbilityDataSO", menuName = "Abilities/Ability Data")]
        
    public class AbilityDataSO : ScriptableObject
    {
        [SerializeField] private string _name;
        [SerializeField] private string _id;
        [SerializeField] private int _count;
        [SerializeField] private string _description;
        [SerializeField] private int _cost;
        
        public string Name => _name;
        public string Id => _id;
        public string Description => _description;
        
        public int Cost => _cost;
        
        
    
        public int Count
        {
            get => _count;
            set => _count = Mathf.Max(0, value); //שלא ירד מתחת ל0
        }
    }
}