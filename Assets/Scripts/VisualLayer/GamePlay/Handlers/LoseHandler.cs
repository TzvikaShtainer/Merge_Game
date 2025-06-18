using System;
using ServiceLayer.Signals.SignalsClasses;
using UnityEngine;
using Zenject;

namespace VisualLayer.GamePlay.Handlers
{
    public class LoseHandler : MonoBehaviour
    {
        [Inject]
        private SignalBus _signalBus;

        public BoxCollider2D detectionZone;
        private bool _isTriggered = false;


        private void Awake()
        {
            _signalBus.Subscribe<OnContinueClicked>(OnPlayerContinueClicked);
        }

        private void OnPlayerContinueClicked()
        {
            _isTriggered = false;
        }

        private void FixedUpdate()
        {
            CheckForPlayerLoseCollisions();
        }

        private void CheckForPlayerLoseCollisions()
        {
            Collider2D hit = Physics2D.OverlapBox(detectionZone.bounds.center, detectionZone.bounds.size, 0f, LayerMask.GetMask("StandingFruit"));
            if (hit)
            {
                //Debug.Log(hit.gameObject.name);
                CustomTriggerBehavior(hit);
            }
        }

        private void CustomTriggerBehavior(Collider2D collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("StandingFruit") && !_isTriggered)
            {
                _signalBus.Fire<HandleItemsCollisionAfterLose>();
                
                _signalBus.Fire<ReachedColliderLose>();

                _isTriggered = true;
            }
        }
    }
}