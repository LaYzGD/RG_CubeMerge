namespace Game.Core
{
    public abstract class EntityState
    {
        protected EntityFSM entityFSM;
        protected EntityStateContext ctx;
        public EntityState(EntityFSM entityFSM, EntityStateContext ctx)
        {
            this.entityFSM = entityFSM;
            this.ctx = ctx;
        }

        public virtual void Enter() { }
        public virtual void FixedUpdate() { }
        public virtual void Update() { }
        public virtual void Exit() { }
    }
}