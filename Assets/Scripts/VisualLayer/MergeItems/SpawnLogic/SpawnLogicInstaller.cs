using UnityEngine;
using Zenject;

namespace VisualLayer.MergeItems.SpawnLogic
{
    [CreateAssetMenu(menuName = "Merge/MergeItems/SpawnLogicInstaller")]
    public class SpawnLogicInstaller : ScriptableObjectInstaller<SpawnLogicInstaller>
    {
        [SerializeField] 
        private Transform _spawnItemsManager;
        
        public override void InstallBindings()
        {
            Container
                .Bind<Transform>()
                .FromComponentInNewPrefab(_spawnItemsManager)
                .AsSingle();
            
            Container
                .Bind<ISpawn>()
                .To<SpawnLogic>()
                .AsSingle();
        }
    }
}