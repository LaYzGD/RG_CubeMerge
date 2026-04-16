using Game.Configs;
using Game.Core;
using Game.Features;
using Game.Features.Spawn;
using Game.Infrastructure;
using Game.Signals;
using Game.Systems.Input;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private GameConfig _config;
    [SerializeField] private InputService _inputService;

    private SignalBus _bus;
    private IMergeService _mergeService;
    private CubeSpawner _spawner;
    private GameFlow _gameFlow;
    private IEntitiesRegistry _entitiesRegistry;

    private void Awake()
    {
        _bus = new SignalBus();
        _entitiesRegistry = new EntitiesRegistry(_bus);
        _mergeService = new MergeService(_bus, _entitiesRegistry, _config.ImpulseThreshold);
        _spawner = new CubeSpawner(_bus, _config, _entitiesRegistry);
        _gameFlow = new GameFlow(_inputService, _spawner, _config);
        _gameFlow.SetDefaultSpawnPoint(_spawnPoint.position);
        _gameFlow.StartGame();
    }
}
