using System;
using ServiceLayer.Signals.SignalsClasses;
using UnityEngine;
using VisualLayer.MergeItems;
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
            _signalBus.Subscribe<OnContinueClickedSignal>(OnPlayerContinueClicked);
            
            _signalBus.Subscribe<DisableLoseCollider>(OnDisableLoseCollider);
            _signalBus.Subscribe<EnableLoseCollider>(OnEnableLoseCollider);

        }
        
        private void OnDestroy()
        {
            _signalBus.TryUnsubscribe<OnContinueClickedSignal>(OnPlayerContinueClicked);
            
            _signalBus.TryUnsubscribe<DisableLoseCollider>(OnDisableLoseCollider);
            _signalBus.TryUnsubscribe<EnableLoseCollider>(OnEnableLoseCollider);
        }
        
        private void OnDisableLoseCollider() => detectionZone.gameObject.SetActive(false);
        private void OnEnableLoseCollider() => detectionZone.gameObject.SetActive(true);

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
            if (collision.gameObject.layer == LayerCache.StandingFruit && !_isTriggered)
            {
                //Debug.Log("CustomTriggerBehavior StandingFruit");
                Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
                
                if (rb != null && rb.linearVelocity.magnitude > 0.1f)
                {
                    return;
                }
                
                Debug.Log(collision.gameObject.name);

                _signalBus.Fire<HandleItemsCollisionAfterLoseSignal>();
                
                _signalBus.Fire<ReachedColliderLoseSignal>();

                _isTriggered = true;
            }
        }
    }
}