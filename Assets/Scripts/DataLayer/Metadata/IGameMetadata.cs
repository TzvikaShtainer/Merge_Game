using UnityEngine;

namespace DataLayer.Metadata
{
    public interface IGameMetadata
    {
        Object GetPrefabForItem(int itemId);
    }
}