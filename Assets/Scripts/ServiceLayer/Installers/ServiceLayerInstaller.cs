﻿using Blast.ServiceLayer.GameScenes;
using ServiceLayer.EffectsService;
using ServiceLayer.GameScenes;
using ServiceLayer.PlayFabService;
using ServiceLayer.TimeControl;
using UnityEngine;
using Zenject;

namespace ServiceLayer.Installers
{
    [CreateAssetMenu(menuName = "Merge/Data/Service Layer Installer", fileName = "Service Layer Installer")]
    public class ServiceLayerInstaller : ScriptableObjectInstaller<ServiceLayerInstaller>
    {
        public override void InstallBindings()
        {
            Container
                .Bind<IGameScenesService>()
                .To<GameScenesService>()
                .AsSingle();
            
            Container
                .Bind<ITimeController>()
                .To<TimeController>()
                .AsSingle();
            
            Container
                .Bind<IEffectsManager>()
                .To<EffectsManager>()
                .AsSingle();

            Container
                .Bind<IServerService>()
                .To<PlayFabService.PlayFabService>()
                .AsSingle();
        }
    }
}