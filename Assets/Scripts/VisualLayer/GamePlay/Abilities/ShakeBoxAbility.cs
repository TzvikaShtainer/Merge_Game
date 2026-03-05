using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DataLayer;
using DataLayer.DataTypes.abilities;
using DG.Tweening;
using ServiceLayer.EffectsService;
using ServiceLayer.MusicService;
using ServiceLayer.Signals.SignalsClasses;
using UnityEngine;
using VisualLayer.MergeItems;
using Zenject;
using Object = UnityEngine.Object;

namespace VisualLayer.GamePlay.Abilities
{
    public class ShakeBoxAbility : BaseAbility
    {
        public ShakeBoxAbility(AbilityDataSO abilityDataSo, LazyInject<IDataLayer> dataLayer) : base(abilityDataSo, dataLayer)
        {
        }
    }
}