using System;
using ServiceLayer.HourlyCoinsService;
using UnityEngine;
using VisualLayer.GamePlay.Buttons;
using Zenject;

namespace VisualLayer.GamePlay.Handlers
{
    public class HourlyCoinsClickHandler : IHourlyCoinsClickHandler
    {
        [Inject]
        private IHourlyCoinsService  _hourlyCoinsService;
        public void Execute()
        {
            if (_hourlyCoinsService.CanClaim(out TimeSpan remaining))
            {
                _hourlyCoinsService.Claim();
            }
            else
            {
                Debug.Log(
                    $"⏳ עוד {remaining.Minutes:D2}:{remaining.Seconds:D2} עד שזמין שוב");
            }
        }
    }
}