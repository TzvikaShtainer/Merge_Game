using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DataLayer;
using DataLayer.DataTypes;
using DataLayer.DataTypes.abilities;
using ServiceLayer.EffectsService;
using ServiceLayer.MusicService;
using ServiceLayer.Signals.SignalsClasses;
using ServiceLayer.TimeControl;
using UnityEngine;
using VisualLayer.MergeItems;
using Zenject;
using Object = UnityEngine.Object;

namespace VisualLayer.GamePlay.Abilities
{
    public class DestroyAllLowestLevelFruitsAbility : BaseAbility
    {
        public DestroyAllLowestLevelFruitsAbility(AbilityDataSO abilityDataSo, LazyInject<IDataLayer> dataLayer) : base(abilityDataSo, dataLayer)
        {
        }
    }
}