using UnityEngine;
using VisualLayer.GamePlay.Handlers;
using Zenject;

namespace VisualLayer.GamePlay.Installers
{
    [CreateAssetMenu(menuName = "Merge/MergeItems/GameLogicInstaller")]
    public class GameLogicInstaller : ScriptableObjectInstaller<GameLogicInstaller>
    {
        public override void InstallBindings()
        {
            
           // Container
            //    .Bind<GameLogicHandler>()
            //    .AsTransient();
            
            
            
            
        }
    }
}