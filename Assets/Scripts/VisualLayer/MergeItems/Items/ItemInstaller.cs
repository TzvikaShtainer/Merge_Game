using UnityEngine;
using Zenject;

namespace VisualLayer.MergeItems
{
    [CreateAssetMenu(menuName = "Merge/MergeItems/ItemInstaller")]
    public class ItemInstaller : ScriptableObjectInstaller<ItemInstaller>
    {
        [SerializeField]
        private GameObject _itemPrefab;
        
        public override void InstallBindings()
        {
            Container
                .BindFactory<Vector3, Item, Item.Factory>()
                .FromComponentInNewPrefab(_itemPrefab)
                .AsSingle();
        }
    }
}