using DataLayer.DataTypes.abilities;

namespace VisualLayer.GamePlay.Abilities
{
    public interface IAbility
    {
        string Id { get; }
        int Count{ get; set; }
        AbilityDataSO Data { get; }
        public void UseAbility();
        public void Buy();

        public void AddAbilityCount(int amountToAdd);
        public bool IsFirstTime();
        void SetFirstTime(bool value);
    }
}