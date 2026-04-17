using UnityEngine;

namespace Game.Core
{
    public class EntityMergedState : EntityState
    {
        public EntityMergedState(EntityFSM entityFSM, EntityStateContext ctx) : base(entityFSM, ctx)
        {
        }

        public override void Enter()
        {
            ctx.Rigidbody.rotation = Quaternion.Euler(0, 0, 0);
            ctx.Rigidbody.AddForce(Vector3.up * ctx.Config.MergedForce, ForceMode.Impulse);
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