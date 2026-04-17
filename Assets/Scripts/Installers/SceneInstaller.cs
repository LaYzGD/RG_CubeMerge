using Game.Configs;
using Game.Core;
using Game.Features;
using Game.Features.Boosters;
using Game.Features.Merge;
using Game.Features.Score;
using Game.Features.Spawn;
using Game.Infrastructure;
using Game.Signals;
using Game.Systems.Input;
using Game.Systems.VFX;
using UnityEngine;
using Zenject;

public class SceneInstaller : MonoInstaller
{
    [SerializeField] private GameConfig _gameConfig;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private InputService _inputService;
    [SerializeField] private GameOverConditionHandler _gameOverHandler;
    public override void InstallBindings()
    {
        Container.Bind<SignalBus>().AsSingle().NonLazy();
        Container.Bind<IGameOverService>().FromInstance(_gameOverHandler).AsSingle().NonLazy();
        Container.Bind<GameConfig>().FromInstance(_gameConfig).AsSingle();
        Container.Bind<IEntitiesRegistry>().To<EntitiesRegistry>().AsSingle().NonLazy();
        Container.Bind<IMergeService>().To<MergeService>().AsSingle().WithArguments(_gameConfig.ImpulseThreshold);
        Container.Bind<IInputService>().FromInstance(_inputService).AsSingle().NonLazy();
        Container.Bind<CubeViewPool>().AsSingle();
        Container.Bind<VFXSpawner>().AsSingle().NonLazy();
        Container.Bind<IEntitySpawnService>().To<CubeSpawner>().AsSingle();
        Container.Bind<IBooster>().To<AutoMergeBooster>().AsSingle().NonLazy();
        Container.Bind<IScoreService>().To<ScoreService>().AsSingle().NonLazy();

        Container.BindInterfacesTo<GameFlow>().AsSingle().WithArguments(_spawnPoint.position);
    }
}