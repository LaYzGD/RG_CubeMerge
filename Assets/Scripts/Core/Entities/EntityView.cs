using UnityEngine;

namespace Game.Core
{
    public abstract class EntityView : MonoBehaviour
    {
        public abstract IMergeable Model { get; }
        public abstract EntityFSM EntityFSM { get; }
        public abstract void SetMerged();
        public abstract void SetAutoMerging();
        public abstract void Init(IMergeable model);
        public abstract void SetNewValue(EntityData data);
        public abstract void Launch(Vector3 force);
        public abstract void Release();
        public abstract void Move(Vector3 pos);
        public abstract void SetKinematic(bool flag);
    }
}