using Game.Configs;
using Game.Core;
using UnityEngine;
using Zenject;

namespace Game.Infrastructure
{
    public class CubeViewPool : Pool<CubeView>
    {
        private readonly DiContainer _container;
        public CubeViewPool(DiContainer container, GameConfig config, int defaultCapacity = 30) : base(defaultCapacity)
        {
            _container = container;
            prefab = config.CubeViewPrefab;
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

        protected override CubeView Instantiate()
        {
            return _container.InstantiatePrefabForComponent<CubeView>(prefab);
        }

        protected override void OnGet(CubeView obj)
        {
            obj.BindRelease(Release);
        }
    }
}