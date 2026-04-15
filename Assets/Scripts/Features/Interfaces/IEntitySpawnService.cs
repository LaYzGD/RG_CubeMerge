using UnityEngine;

namespace Game.Features
{
    public interface IEntitySpawnService<T>
    {
        public T Spawn(Vector3 pos);
    }
}