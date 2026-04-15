using Game.Configs;
using Game.Features;
using Game.Features.Spawn;
using Game.Infrastructure;
using Game.Signals;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] private GameConfig _config;

    private SignalBus _bus;
    private IMergeService _mergeService;
    private CubeSpawner _spawner;

    private void Awake()
    {
        _bus = new SignalBus();
        _mergeService = new MergeService(_bus, _config.ImpulseThreshold);
        _spawner = new CubeSpawner(_bus, _config.SpawnConfig, _config.EntityConfig, _config.CubeViewPrefab);
    }

    private void Start()
    {

    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _spawner.Spawn(UnityEngine.Random.insideUnitSphere);
        }
    }
}
