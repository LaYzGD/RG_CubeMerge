using UnityEngine;
using Game.Systems.Input;
using Game.Configs;
using System;
using Cysharp.Threading.Tasks;
using System.Threading;
using Game.Features.Spawn;
using Game.Features;
using Game.Signals;
using Game.Features.Score;

namespace Game.Core
{
    public class GameFlow : IDisposable
    {
        private IInputService _inputService;
        private IEntitySpawnService _entitySpawnService;
        private EntityView _currentObject;
        private GameConfig _gameConfig;
        private IGameOverService _gameOverHandler;
        private Camera _mainCam;
        private Vector3 _spawnPoint;
        private IScoreService _scoreService;
        private SignalBus _bus;
        private bool _isPressed = false;
        private Plane _dragPlane;
        private bool _planeInitialized;
        private CancellationTokenSource _cts;

        public GameFlow(SignalBus bus, IInputService inputs, IEntitySpawnService spawnService, IScoreService score, 
            GameConfig config, IGameOverService gameOverHandler, Vector3 spawnPoint)
        {
            _bus = bus;
            _inputService = inputs;
            _entitySpawnService = spawnService;
            _gameConfig = config;
            _gameOverHandler = gameOverHandler;
            _scoreService = score;
            _spawnPoint = spawnPoint;
            _inputService.OnPress += OnPress;
            _inputService.OnDrag += MoveCurrentEntity;
            _inputService.OnRelease += LaunchCurrentEntity;
            _mainCam = Camera.main;

            StartGame();
        }

        private void StartGame()
        {
            _cts = new CancellationTokenSource();
            Run(_cts.Token).Forget();
        }

        private void SpawnNewEntity(Vector3 pos)
        {
            var obj = _entitySpawnService.Spawn(pos);
            _currentObject = obj;
        }

        private async UniTaskVoid Run(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                if (CheckGameOver())
                    break;

                SpawnNewEntity(_spawnPoint);

                await WaitUntilLaunched(token);

                await UniTask.Delay(800, cancellationToken: token);
            }
        }

        private bool CheckGameOver()
        {
            if (!_gameOverHandler.IsGameOver())
                return false;

            _bus.Invoke(new GameOverSignal(_scoreService.CurrentScore));
            return true;
        }

        private async UniTask WaitUntilLaunched(CancellationToken token)
        {
            await UniTask.WaitUntil(() => _currentObject == null, cancellationToken: token);
        }

        private void OnPress()
        {
            _isPressed = true;
            _planeInitialized = false;
        }

        private void MoveCurrentEntity(Vector2 delta)
        {
            if (_currentObject == null || !_isPressed)
                return;

            if (!_planeInitialized)
            {
                _dragPlane = new Plane(Vector3.up, _currentObject.transform.position);

                Vector2 screenPos = UnityEngine.InputSystem.Pointer.current.position.ReadValue();
                Ray ray = _mainCam.ScreenPointToRay(screenPos);

                if (_dragPlane.Raycast(ray, out float enter))
                {
                    var world = ray.GetPoint(enter);
                }

                _planeInitialized = true;
            }

            Vector2 pointer = UnityEngine.InputSystem.Pointer.current.position.ReadValue();
            Ray moveRay = _mainCam.ScreenPointToRay(pointer);

            if (_dragPlane.Raycast(moveRay, out float moveEnter))
            {
                Vector3 world = moveRay.GetPoint(moveEnter);

                float targetX = world.x;

                Vector3 pos = _currentObject.transform.position;
                pos.x = Mathf.Clamp(targetX, -_gameConfig.ClampX, _gameConfig.ClampX);

                _currentObject.Move(pos);
            }
        }

        private void LaunchCurrentEntity()
        {
            if (_currentObject == null)
            {
                return;
            }

            _currentObject.Launch(Vector3.forward * _gameConfig.LaunchForce);
            _isPressed = false;
            _planeInitialized = false;
            _currentObject = null;
        }

        public void Dispose()
        {
            _cts.Cancel();
            _cts.Dispose();
            _inputService.OnPress -= OnPress;
            _inputService.OnDrag -= MoveCurrentEntity;
            _inputService.OnRelease -= LaunchCurrentEntity;
        }
    }
}