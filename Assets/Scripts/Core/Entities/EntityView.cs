using System;
using UnityEngine;

namespace Game.Core
{
    public abstract class EntityView : MonoBehaviour
    {
        public abstract void Init(EntityModel model);
        public abstract void SetNewValue(EntityData data);
        public abstract void Launch(Vector3 force);
    }
}