namespace Game.Core
{
    public class EntityDraggingState : EntityState
    {
        public EntityDraggingState(EntityFSM entityFSM, EntityStateContext ctx) : base(entityFSM, ctx)
        {
        }

        public override void Enter()
        {
            ctx.Rigidbody.linearVelocity = UnityEngine.Vector3.zero;
        } 
    }
}