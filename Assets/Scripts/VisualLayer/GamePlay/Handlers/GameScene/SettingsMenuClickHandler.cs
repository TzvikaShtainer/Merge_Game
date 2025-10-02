using System;
using Cysharp.Threading.Tasks;
using DataLayer.DataTypes;
using ServiceLayer.GameScenes;
using ServiceLayer.Signals.SignalsClasses;
using ServiceLayer.TimeControl;
using UnityEngine;
using VisualLayer.GamePlay.Popups.MusicMenuPopup;
using VisualLayer.GamePlay.Popups.YesNoPopup;
using VisualLayer.Loader;
using Zenject;

namespace VisualLayer.GamePlay.Handlers
{
    public class SettingsMenuClickHandler : ISettingsMenuClickHandler
    {
        [Inject] 
        private SettingsMenuPopup.Factory _musicMenuPopupFactory;
        
        [Inject]
        private ITimeController _timeController;
        
        [Inject]
        private SignalBus _signalBus;
        
        [Inject] 
        private ISettingsMenuActions _actions;
        
        public async UniTask Execute()
        {
            //_timeController.PauseGameplay();
            //_signalBus.Fire<PauseInput>();
            
            var popup = _musicMenuPopupFactory.Create();
            popup.Initialize(_actions);
            
            //_timeController.UnpauseGameplay();
            
        }
    }
}