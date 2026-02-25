using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DataLayer.DataTypes;
using ServiceLayer.EffectsService;
using ServiceLayer.MusicService;
using ServiceLayer.Signals.SignalsClasses;
using UnityEngine;
using VisualLayer.MergeItems;
using Zenject;
using Object = UnityEngine.Object;

namespace VisualLayer.GamePlay.Abilities
{
    public class DestroyItemsAfterContinue : BaseAbility
    {
        [Inject] 
        private IEffectsManager _effectsManager;
        
        [Inject]
        private ISfxService  _sfxService;

        public override async void UseAbility()
        {
            DisableEnvironment();

            FindAndDestroyTwoLowestItems();    
            
            await UniTask.Delay(TimeSpan.FromSeconds(1.5));
            
            EnableEnvironment();
        }
        
        protected override void DisableEnvironment()
        {
            base.DisableEnvironment();
            
            SignalBus.Fire<PauseInputSignal>();
        }

        
        private void FindAndDestroyTwoLowestItems()
        {
            List<Item> itemsInJar = Object.FindObjectsOfType<Item>()
                .Where(item => !IsOutsideTheJar(item))
                .ToList();

            if (itemsInJar.Count == 0) return;

            var distinctIds = itemsInJar
                .Select(item => item.GetItemId())
                .Distinct()
                .OrderBy(id => id)
                .Take(2) 
                .ToList();

            if (distinctIds.Count == 0) return;

            foreach (int idToDestroy in distinctIds)
            {
                var targets = itemsInJar.Where(i => i.GetItemId() == idToDestroy).ToList();
        
                foreach (var target in targets)
                {
                    ExecuteDestruction(target);
                }
            }
        }

        private void ExecuteDestruction(Item item)
        {
            if (item == null) return;
    
            _effectsManager.PlayEffect(EffectType.DestroyAbility, item.transform.position);
            _sfxService.PlaySfxType(SfxType.DestroyAbility);
            Object.Destroy(item.gameObject);
        }

        private List<Item> RemoveItemsThatNotInTheJar(List<Item> allItems)
        {
            allItems = allItems
                .Where(item => !IsOutsideTheJar(item))
                .ToList();
            return allItems;
        }
        
        public override void EnableEnvironment()
        {
            SignalBus.Fire<UnpauseInputSignal>();
            SignalBus.Fire<EnableUISignal>();
            
            EnableItemsOutsideTheJar();
        }
    }
}