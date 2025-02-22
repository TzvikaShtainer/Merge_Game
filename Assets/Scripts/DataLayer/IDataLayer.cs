using DataLayer.Balances;
using DataLayer.Metadata;

namespace DataLayer
{
    public interface IDataLayer
    {
        IGameMetadata Metadata { get; }
        
        IPlayerBalances Balances { get; }
    }
}