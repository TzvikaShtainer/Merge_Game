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

        private void Awake()
        {
            detectionZone = GetComponent<BoxCollider2D>();
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
                //Debug.Log("Player detected!");
                CustomTriggerBehavior(hit);
            }
        }

        private void CustomTriggerBehavior(Collider2D collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("StandingFruit"))
            {
                _signalBus.Fire<HandleItemsCollisionAfterLose>();
                _signalBus.Fire<ReachedColliderLose>();
            }
        }
    }
}