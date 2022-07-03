using UnityEngine;

namespace PlayerStateMachine
{
    public class TimeElapsedTransition : Transition
    {
        [SerializeField] private float _transitionTime;

        private float _elapsedTime;

        private void OnEnable()
        {
            _elapsedTime = 0;
        }

        private void Update()
        {
            _elapsedTime += Time.deltaTime;

            if (_elapsedTime >= _transitionTime)
                IsNeedTransit = true;
        }
    }
}