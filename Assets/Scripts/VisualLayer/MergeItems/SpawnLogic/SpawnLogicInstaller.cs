using UnityEngine;
using Zenject;

namespace VisualLayer.MergeItems.SpawnLogic
{
    [CreateAssetMenu(menuName = "Merge/MergeItems/SpawnLogicInstaller")]
    public class SpawnLogicInstaller : ScriptableObjectInstaller<SpawnLogicInstaller>
    {
        public override void InstallBindings()
        {
            Container
                .Bind<ISpawn>()
                .To<SpawnLogic>()
                .AsSingle();
        }
    }
}