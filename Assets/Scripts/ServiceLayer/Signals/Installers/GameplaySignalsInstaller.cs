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

            //Items Signals
            Container.DeclareSignal<ItemMergedSignal>();
            Container.DeclareSignal<AddCoinsSignal>();
            
            
            //Input Signals
            Container.DeclareSignal<UnpauseInput>();
            Container.DeclareSignal<PauseInput>();
            
            
            //UI Signals
            Container.DeclareSignal<EnableUI>();
            Container.DeclareSignal<DisableUI>();

            
            //Psychics Signals
            Container.DeclareSignal<ReachedColliderLose>();
            Container.DeclareSignal<HandleItemsCollisionAfterLose>();
            
            Container.DeclareSignal<OnContinueClicked>();
            
        }
    }
}