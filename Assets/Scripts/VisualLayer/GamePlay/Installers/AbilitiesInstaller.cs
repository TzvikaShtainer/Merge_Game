using System.Collections.Generic;
using DataLayer.DataTypes.abilities;
using UnityEngine;
using VisualLayer.GamePlay.Abilities;
using Zenject;

namespace VisualLayer.GamePlay.Installers
{
    public class AbilitiesInstaller: MonoInstaller<AbilitiesInstaller>
    {
        [SerializeField]
        private GameObject shakeBoxAbilityJarPrefab;

        [SerializeField] 
        private Camera mainGameplayCamera;
        
        [Inject] private AbilityManager _abilityManager;
        [Inject] private List<IAbility> _allAbilities;
        
        public override void InstallBindings()
        {
            Container
                .Bind<Transform>()
                .WithId("ShakeBoxJar")
                .FromInstance(shakeBoxAbilityJarPrefab.transform)
                .AsCached();
                    
            Container
                .Bind<Camera>()
                .WithId("MainGameplayCamera")
                .FromInstance(mainGameplayCamera)
                .AsSingle();
            
            Container.BindInterfacesAndSelfTo<AbilitySceneExecutor>()
                .FromNewComponentOnNewGameObject()
                .AsSingle()
                .NonLazy();
        }
    }
}