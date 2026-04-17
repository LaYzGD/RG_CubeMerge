namespace Game.Core
{
    public class EntityMovingState : EntityState
    {
        public EntityMovingState(EntityFSM entityFSM, EntityStateContext ctx) : base(entityFSM, ctx)
        {
        }

        public override void FixedUpdate()
        {
            if (ctx.Rigidbody.linearVelocity.magnitude < ctx.Config.MovementThreshold)
            {
                entityFSM.ChangeState(new EntityIdleState(entityFSM, ctx));
            }
        }
    }
}