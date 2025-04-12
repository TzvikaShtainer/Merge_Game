namespace VisualLayer.GamePlay.Abilities
{
    public interface IAbility
    {
        string Id { get; }
        int Count{ get; set; }
        
        public void UseAbility();
        public void Buy();
    }
}