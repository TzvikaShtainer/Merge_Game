using UnityEngine;
using Zenject;

namespace VisualLayer.MergeItems
{
    [CreateAssetMenu(menuName = "Merge/MergeItems/ItemInstaller")]
    public class ItemInstaller : ScriptableObjectInstaller<ItemInstaller>
    {
        [SerializeField]
        private GameObject itemPrefab;
        
        public override void InstallBindings()
        {
            Container
                .BindFactory<Item, Item.Factory>()
                .FromComponentInNewPrefab(itemPrefab)
                .AsSingle();
        }
    }
}