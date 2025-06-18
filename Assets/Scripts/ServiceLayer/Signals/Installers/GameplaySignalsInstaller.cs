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
            //Debug.Log("INSTALLING SIGNAL BUS FROM GAMEPLAYSIGNALSINSTALLER", this);
            SignalBusInstaller.Install(Container);

            //Items Signals
            Container.DeclareSignal<ItemMergedSignal>();
            Container.DeclareSignal<AddCoinsSignal>();
            
            
            //Input Signals
            Container.DeclareSignal<UnpauseInputSignal>();
            Container.DeclareSignal<PauseInputSignal>();
            
            
            //UI Signals
            Container.DeclareSignal<EnableUISignal>();
            Container.DeclareSignal<DisableUISignal>();

            
            //Psychics Signals
            Container.DeclareSignal<ReachedColliderLoseSignal>();
            Container.DeclareSignal<HandleItemsCollisionAfterLoseSignal>();
            
            Container.DeclareSignal<OnContinueClickedSignal>();
            
            Container.DeclareSignal<GamePlayReadySignal>();
            //Debug.Log("FINISH SIGNAL BUS");
            
        }
    }
}