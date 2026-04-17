namespace Game.Core
{
    public class EntityFSM
    {
        public EntityState CurrentState { get; private set; }

        public void ChangeState(EntityState newState)
        {
            if (newState == CurrentState) return;

            CurrentState?.Exit();
            CurrentState = newState;
            CurrentState?.Enter();
        }

        public void Update()
        {
            CurrentState?.Update();
        }

        public void FixedUpdate()
        {
            CurrentState?.FixedUpdate();
        }
    }
}