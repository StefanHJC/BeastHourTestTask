using UnityEngine;
using Mirror;

namespace PlayerStateMachine
{
    [RequireComponent(typeof(StateMachine))]
    public abstract class Transition : NetworkBehaviour
    {
        [SerializeField] private State targetState;

        protected Player player;

        public State TargetState => targetState;
        public bool IsNeedTransit { get; protected set; }
        
        private void OnEnable()
        {
            Init();
        }

        protected virtual void Awake()
        {
            player = GetComponent<Player>();
        }

        public virtual void Init()
        {
            IsNeedTransit = false;
        }
    }
}
