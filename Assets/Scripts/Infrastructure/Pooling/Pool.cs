using UnityEngine;
using UnityEngine.Pool;

namespace Game.Infrastructure
{
    public abstract class Pool<T> where T : MonoBehaviour, IPoolable<T>
    {
        protected ObjectPool<T> pool;

        public Pool(T prefab, int defaultCapacity = 30)
        {
            pool = new ObjectPool<T>(() =>
                Object.Instantiate(prefab),
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

        public abstract T GetObject();

        protected abstract void OnGet(T obj);

        public abstract void Release(T obj);
    }
}
