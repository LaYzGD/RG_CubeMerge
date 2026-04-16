using System;
using Game.Configs;
using Game.Core;
using Game.Infrastructure;
using Game.Signals;
using UnityEngine;

namespace Game.Features.Spawn
{
    public class CubeSpawner : IEntitySpawnService, IDisposable
    {
        private CubeViewPool _pool;
        private GameConfig _gameConfig;
        private SignalBus _bus;
        private IEntitiesRegistry _entitiesRegistry;
        
        public CubeSpawner(SignalBus bus, GameConfig config, IEntitiesRegistry registry) 
        {
            _bus = bus;
            _gameConfig = config;
            _pool = new CubeViewPool(_gameConfig.CubeViewPrefab);
            _entitiesRegistry = registry;

            _bus.Subscribe<MergeSignal>(SpawnMergedCube);
        }

        public EntityView Spawn(Vector3 pos)
        {
            var view = GetViewAndBindModel(GetModel(_gameConfig.SpawnConfig));
            view.transform.position = pos;
            view.ResetState();
            return view;
        }

        private void SpawnMergedCube(MergeSignal signal)
        {
            var view = GetViewAndBindModel(new EntityModel(signal.Value));
            _entitiesRegistry.Register(view);
            view.transform.position = signal.Position;
            view.SetKinematic(false);
        }

        private CubeView GetViewAndBindModel(IMergeable model)
        {
            var view = _pool.GetObject();
            view.Init(model);
            view.SetBus(_bus);
            view.SetNewValue(new EntityData(model.Value, GetColor(_gameConfig.EntityConfig, model.Value)));

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
            _bus.Unsubscribe<MergeSignal>(SpawnMergedCube);
        }
    }
}