using System;
using DataLayer.DataTypes;
using ServiceLayer.HourlyCoinsService;
using Unity.VisualScripting;
using UnityEngine;
using VisualLayer.Factories;
using VisualLayer.GamePlay.Buttons;
using VisualLayer.GamePlay.Handlers;
using VisualLayer.GamePlay.PlayerInput;
using VisualLayer.MergeItems;
using VisualLayer.MergeItems.MergeSystem;
using Zenject;

namespace VisualLayer.GamePlay.Installers
{
    public class StartScreenInstaller : MonoInstaller<StartScreenInstaller>
    {
        [SerializeField] 
        private GameLevelType _levelType;
    
        public override void InstallBindings()
        {
            //Debug.Log("InstallBindings Started");
            
            // Container
            //     .Bind<IPlayerInput>()
            //     .To<DesktopInputManager>()
            //     .AsSingle()
            //     .IfNotBound();
        
            Container
                .Bind<GameLevelType>()
                .FromInstance(_levelType)
                .AsSingle();
        
            Container
                .Bind<IStartGameClickHandler>()
                .To<StartGameClickHandler>()
                .AsSingle();
            
            Container
                .Bind<IHourlyCoinsClickHandler>()
                .To<HourlyCoinsClickHandler>()
                .AsSingle();
            
            //Debug.Log("InstallBindings Ended");
        }
    }
}