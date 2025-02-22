using DataLayer.Balances;
using DataLayer.Metadata;
using Zenject;

namespace DataLayer
{
    public class DataLayer : IDataLayer
    {
        [Inject]
        public IGameMetadata Metadata { get; private set; }
        
        [Inject]
        public IPlayerBalances Balances { get; }
    }
}