using UnityEngine;

namespace Game.Core
{
    public class EntityIdleState : EntityState
    {
        public EntityIdleState(EntityFSM entityFSM, EntityStateContext ctx) : base(entityFSM, ctx)
        {
        }

        public override void Enter()
        {
            ctx.Rigidbody.isKinematic = false;
            ctx.Rigidbody.linearVelocity = Vector3.zero;
            ctx.Rigidbody.constraints = RigidbodyConstraints.None;
            ctx.Trail.Stop();
        }

        public override void FixedUpdate()
        {
            if (ctx.Rigidbody.linearVelocity.magnitude > ctx.Config.MovementThreshold)
            {
                entityFSM.ChangeState(new EntityMovingState(entityFSM, ctx));
            }
        }
    }
}