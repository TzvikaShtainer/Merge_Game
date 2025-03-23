using DataLayer.DataTypes;
using UnityEngine;

namespace DataLayer.Metadata
{
    public interface IGameMetadata
    {
        Object GetPrefabForItem(int itemId);
        bool HasNextLevelItem(int itemId);

        public ItemMetadata GetItemMetadata(int itemIdToFind);
        
        GameLevelMetadata GetLevelMetadata(GameLevelType levelType);
		
        InfraScreenMetadata GetInfraScreenMetadata(InfraScreenType screenType);
        
    }
}