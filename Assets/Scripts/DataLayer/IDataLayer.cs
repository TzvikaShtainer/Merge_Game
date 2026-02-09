using DataLayer.Balances;
using DataLayer.Metadata;
using VisualLayer.GamePlay.Abilities;

namespace DataLayer
{
    public interface IDataLayer
    {
        IGameMetadata Metadata { get; }
        
        IPlayerBalances Balances { get; }
        
        AbilityManager AbilityManager { get; }
    }
}