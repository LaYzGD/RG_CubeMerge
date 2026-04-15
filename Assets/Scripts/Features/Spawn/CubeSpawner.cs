using System;
using Game.Configs;
using Game.Core;
using Game.Infrastructure;
using Game.Signals;
using UnityEngine;

namespace Game.Features.Spawn
{
    public class CubeSpawner : IEntitySpawnService<CubeView>, IDisposable
    {
        private CubeViewPool _pool;
        private SpawnConfig _spawnConfig;
        private EntityConfig _entityConfig;
        private SignalBus _bus;
        
        public CubeSpawner(SignalBus bus, SpawnConfig spawnConfig, EntityConfig config, CubeView prefab) 
        {
            _bus = bus;
            _spawnConfig = spawnConfig;
            _entityConfig = config;
            _pool = new CubeViewPool(prefab);

            _bus.Subscribe<CreateMergedEntitySignal>(SpawnMergedCube);
        }

        public CubeView Spawn(Vector3 pos)
        {
            var view = GetViewAndBindModel(GetModel(_spawnConfig));
            view.transform.position = pos;

            return view;
        }

        private void SpawnMergedCube(CreateMergedEntitySignal signal)
        {
            var view = GetViewAndBindModel(new EntityModel(signal.Value));
            view.transform.position = signal.Position;
        }

        private CubeView GetViewAndBindModel(IMergeable model)
        {
            var view = _pool.GetObject();
            view.Init(model);
            view.SetBus(_bus);
            view.SetNewValue(new EntityData(model.Value, GetColor(_entityConfig, model.Value)));

            return view;
        }

        private Color GetColor(EntityConfig config, int value)
        {
            var data = config.Database;
            if (value <= 0)
                return data[data.Count - 1].Color;

            int index = GetPowerOfTwoIndex(value);

            if (index >= 0 && index < data.Count)
                return data[index].Color;

            return data[data.Count - 1].Color;
        }

        private int GetPowerOfTwoIndex(int value)
        {
            int index = 0;

            while (value > 1)
            {
                value >>= 1;
                index++;
            }

            return index;
        }

        private IMergeable GetModel(SpawnConfig config)
        {
            float totalChance = 0f;

            foreach (var item in config.SpawnDatas)
            {
                totalChance += item.SpawnChance;
            }

            float randomPoint = UnityEngine.Random.value * totalChance;

            float current = 0f;
            foreach (var item in config.SpawnDatas)
            {
                current += item.SpawnChance;
                if (randomPoint <= current)
                {
                    return new EntityModel(item.Value);
                }
            }

            return new EntityModel(config.SpawnDatas[config.SpawnDatas.Count - 1].Value);
        }

        public void Dispose()
        {
            _bus.Unsubscribe<CreateMergedEntitySignal>(SpawnMergedCube);
        }
    }
}