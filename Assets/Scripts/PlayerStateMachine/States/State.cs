using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace PlayerStateMachine
{
    [RequireComponent(typeof(StateMachine))]
    public abstract class State : NetworkBehaviour
    {
        [SerializeField] private List<Transition> _transitions;

        protected Player player;

        public virtual void Enter()
        {
            enabled = true;

            foreach (var transition in _transitions)
            {
                transition.enabled = true;
                transition.Init();
            }
        }

        public void Exit()
        {
            if (enabled)
            {
                foreach (var transition in _transitions)
                {
                    transition.enabled = false;
                }
                enabled = false;
            }
        }

        public State GetNext()
        {
            foreach (var transition in _transitions)
                if (transition.IsNeedTransit)
                    return transition.TargetState;

            return null;
        }
        
        protected virtual void Awake()
        {
            player = GetComponent<Player>();
        }
    }
}
