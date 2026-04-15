namespace Game.Core.FSM
{
    public class EntityStateMachine
    {
        private State _currentState;

        public State CurrentState => _currentState;

        public void ChangeState(State newState)
        {
            if (_currentState == null)
            {
                _currentState = newState;
                _currentState.Enter();
                return;
            }

            if (_currentState == newState) return;

            _currentState.Exit();
            _currentState = newState;
            _currentState.Enter();
        }

        public void Update()
        {
            _currentState?.Update();
        }

        public void FixedUpdate()
        {
            _currentState?.FixedUpdate();
        }
    }
}