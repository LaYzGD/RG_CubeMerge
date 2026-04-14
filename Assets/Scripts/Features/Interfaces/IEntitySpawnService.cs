using UnityEngine;

namespace Game.Features
{
    public interface IEntitySpawnService<T>
    {
        public T Spawn(int value, Vector3 pos);
    }
}