using Game.Core;
using UnityEngine;

namespace Game.Infrastructure
{
    public class CubeViewPool : Pool<CubeView>
    {
        public CubeViewPool(CubeView prefab, int defaultCapacity = 30) : base(prefab, defaultCapacity)
        {
        }

        public override CubeView GetObject()
        {
            if (pool == null)
            {
                Debug.LogError("Pool is not initialized");
                return null;
            }

            var obj = pool.Get();
            return obj;
        }

        public override void Release(CubeView obj)
        {
            pool.Release(obj);
        }

        protected override void OnGet(CubeView obj)
        {
            obj.BindRelease(Release);
        }
    }
}