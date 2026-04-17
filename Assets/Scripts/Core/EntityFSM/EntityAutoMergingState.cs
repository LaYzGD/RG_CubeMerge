namespace Game.Core
{
    public class EntityAutoMergingState : EntityState
    {
        public EntityAutoMergingState(EntityFSM entityFSM, EntityStateContext ctx) : base(entityFSM, ctx)
        {
        }

        public override void Enter()
        {
            ctx.Trail.Play();
        }
    }
}