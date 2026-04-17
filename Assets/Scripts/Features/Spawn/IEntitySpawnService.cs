using Game.Core;
using UnityEngine;

namespace Game.Features.Spawn
{
    public interface IEntitySpawnService
    {
        public EntityView Spawn(Vector3 pos);
    }
}