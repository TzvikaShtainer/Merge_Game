
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DataLayer;
using DataLayer.DataTypes;
using DataLayer.DataTypes.abilities;
using ServiceLayer.EffectsService;
using ServiceLayer.MusicService;
using ServiceLayer.Signals.SignalsClasses;
using UnityEngine;
using VisualLayer.Components.UI;
using VisualLayer.Factories;
using VisualLayer.GamePlay.PlayerInput;
using VisualLayer.MergeItems;
using Zenject;
using Object = UnityEngine.Object;

namespace VisualLayer.GamePlay.Abilities
{
    public class DestroySpecificFruitAbility : BaseAbility
    {
        public DestroySpecificFruitAbility(AbilityDataSO abilityDataSo, LazyInject<IDataLayer> dataLayer) : base(abilityDataSo, dataLayer)
        {
        }
    }
}