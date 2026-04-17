using Game.Systems.VFX;
using UnityEngine;

namespace Game.Infrastructure
{
    public class VFXPool : Pool<VFX>
    {
        public VFXPool(VFX prefab) 
        {
            this.prefab = prefab;
        }

        public override VFX GetObject()
        {
            return pool.Get();
        }

        public override void Release(VFX obj)
        {
            pool.Release(obj);
        }

        protected override VFX Instantiate()
        {
            return Object.Instantiate(prefab);
        }

        protected override void OnGet(VFX obj)
        {
            obj.BindRelease(Release);
        }
    }
}