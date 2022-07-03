using UnityEngine;
using Mirror;

namespace PlayerStateMachine
{
    [RequireComponent(typeof(Player))]
    public class StateMachine : NetworkBehaviour
    {
        [SerializeField] private State _startState;

        private State _currentState;

        public State CurrentState => _currentState;

        private void Start()
        {
            Reset(_startState);
        }

        private void Update()
        {
            if (_currentState == null)
                return;

            State nextState = _currentState.GetNext();

            if (nextState != null && isServer == false)
                CmdTransit(nextState);
            else if (nextState != null && isLocalPlayer == true)
                CmdTransit(nextState);
        }

        private void Reset(State startState)
        {
            _currentState = startState;

            if (_currentState != null)
                _currentState.Enter();
        }

        [ClientRpc]
        private void RpcTransit(State nextState)
        {
            if (_currentState != null)
                _currentState.Exit();

            _currentState = nextState;
            _currentState.Enter();
        }

        [Command]
        private void CmdTransit(State nextState)
        {
            if (_currentState != null)
                _currentState.Exit();

            _currentState = nextState;
            _currentState.Enter();

            RpcTransit(nextState);
        }
    }
}