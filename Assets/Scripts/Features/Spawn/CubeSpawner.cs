using Game.Configs;
using Game.Core;
using Game.Infrastructure;
using UnityEngine;

namespace Game.Features.Spawn
{
    public class CubeSpawner : IEntitySpawnService<CubeView>
    {
        private CubeViewPool _pool;
        private SpawnConfig _spawnConfig;
        private EntityConfig _entityConfig;
        
        public CubeSpawner(SpawnConfig spawnConfig, EntityConfig config, CubeView prefab) 
        {
            _spawnConfig = spawnConfig;
            _entityConfig = config;
            _pool = new CubeViewPool(prefab);
        }

        public CubeView Spawn(int value, Vector3 pos)
        {
            var model = GetModel(_spawnConfig);
            var view = _pool.GetObject();

            view.Init(model);
            view.SetNewValue(new EntityData(value, GetColor(_entityConfig, value)));
            view.transform.position = pos;

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

        private EntityModel GetModel(SpawnConfig config)
        {
            float totalChance = 0f;

            foreach (var item in config.SpawnDatas)
            {
                totalChance += item.SpawnChance;
            }

            float randomPoint = Random.value * totalChance;

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
    }
}