using UnityEngine;

namespace ServiceLayer.Utilis
{
    public class DelayTimer
    {
        private float _delayDuration;
        private float _lastActionTime = Mathf.NegativeInfinity;
        
        public DelayTimer(float delayDuration)
        {
            _delayDuration = delayDuration;
        }

        public bool IsReady => Time.time - _lastActionTime >= _delayDuration;

        public void Reset()
        {
            _lastActionTime = Time.time;
        }

        public void SetDelay(float newDelay)
        {
            _delayDuration = newDelay;
        }
    }
}