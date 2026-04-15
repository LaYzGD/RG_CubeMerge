using Game.Signals;
using UnityEngine;

namespace Game.Core
{
    public abstract class EntityView : MonoBehaviour
    {
        public abstract IMergeable Model { get; }
        public abstract void Init(IMergeable model);
        public abstract void SetNewValue(EntityData data);
        public abstract void Launch(Vector3 force);
        public abstract void Release();
    }
}