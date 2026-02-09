using DataLayer.Balances;
using DataLayer.Metadata;
using VisualLayer.GamePlay.Abilities;
using Zenject;

namespace DataLayer
{
    public class DataLayer : IDataLayer
    {
        [Inject]
        public IGameMetadata Metadata { get; private set; }
        
        [Inject]
        public IPlayerBalances Balances { get; }
        
        [Inject]
        public AbilityManager AbilityManager { get; }
    }
}