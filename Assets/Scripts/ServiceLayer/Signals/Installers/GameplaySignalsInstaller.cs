using ServiceLayer.Signals.SignalsClasses;
using UnityEngine;
using Zenject;

namespace ServiceLayer.Signals.Installers
{
    [CreateAssetMenu(menuName = "Merge/Signals/Gameplay Signals", fileName = "Gameplay Signals")]
    public class GameplaySignalsInstaller : ScriptableObjectInstaller<GameplaySignalsInstaller>
    {
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);

            Container.DeclareSignal<ItemMergedSignal>();
            
            Container.DeclareSignal<UnpauseInput>();
            Container.DeclareSignal<PauseInput>();
            
            Container.DeclareSignal<EnableUI>();
            Container.DeclareSignal<DisableUI>();

            Container.DeclareSignal<ReachedColliderLose>();
            Container.DeclareSignal<HandleItemsCollisionAfterLose>();
        }
    }
}