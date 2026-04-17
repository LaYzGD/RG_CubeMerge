using System;
using System.Collections.Generic;
using Game.Configs;
using Game.Infrastructure;
using Game.Signals;
using UnityEngine;

namespace Game.Systems.VFX
{
    public class VFXSpawner : IDisposable
    {
        private SignalBus _bus;
        private Dictionary<VFXType, VFXPool> _pools;
        private VFXConfig _config;
        public VFXSpawner(SignalBus bus, GameConfig config) 
        {
            _bus = bus;
            _config = config.VFXConfig;
            _pools = new Dictionary<VFXType, VFXPool>();

            _bus.Subscribe<CreateVFXSignal>(Spawn);
        }

        public void Dispose()
        {
            _bus.Unsubscribe<CreateVFXSignal>(Spawn);
        }

        private void Spawn(CreateVFXSignal signal) 
        {
            var type = signal.VFXType;
            
            if (!_pools.TryGetValue(type, out VFXPool pool))
            {
                var prefab = GetPrefabFromConfigByType(type);
                if (prefab == null)
                {
                    return;
                }

                var newPool = new VFXPool(prefab);
                _pools.Add(type, newPool);
                CreateVFXFromPool(newPool, signal.Position);
                return;
            }

            CreateVFXFromPool(pool, signal.Position);
        }

        private void CreateVFXFromPool(VFXPool pool, Vector3 pos) 
        {
            var vfx = pool.GetObject();
            vfx.transform.position = pos;
            vfx.PlayVFX();
        }

        private VFX GetPrefabFromConfigByType(VFXType type)
        {
            foreach (var vfxEntry in _config.VfxEntries) 
            {
                if (vfxEntry.Type == type)
                {
                    return vfxEntry.VFX;
                }
            }

            return null;
        }
    }
}