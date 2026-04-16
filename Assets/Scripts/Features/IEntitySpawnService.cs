using Game.Core;
using UnityEngine;

namespace Game.Features
{
    public interface IEntitySpawnService
    {
        public EntityView Spawn(Vector3 pos);
    }
}