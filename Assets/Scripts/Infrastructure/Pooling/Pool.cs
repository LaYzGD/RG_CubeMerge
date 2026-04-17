using UnityEngine;
using UnityEngine.Pool;

namespace Game.Infrastructure
{
    public abstract class Pool<T> where T : MonoBehaviour, IPoolable<T>
    {
        protected ObjectPool<T> pool;
        protected T prefab;

        public Pool(int defaultCapacity = 30)
        {
            pool = new ObjectPool<T>(() =>
                Instantiate(),
                (ent) =>
                {
                    ent.gameObject.SetActive(true);
                    OnGet(ent);
                },
                (ent) => ent.gameObject.SetActive(false),
                (ent) => Object.Destroy(ent.gameObject),
                false,
                defaultCapacity);
        }

        protected abstract T Instantiate();

        public abstract T GetObject();

        protected abstract void OnGet(T obj);

        public abstract void Release(T obj);
    }
}
