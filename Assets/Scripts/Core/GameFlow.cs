using UnityEngine;
using Game.Features;
using Game.Systems.Input;
using Game.Configs;
using System;
using Cysharp.Threading.Tasks;
using System.Threading;

namespace Game.Core
{
    public class GameFlow : IDisposable
    {
        private IInputService _inputService;
        private IEntitySpawnService _entitySpawnService;
        private EntityView _currentObject;
        private GameConfig _gameConfig;
        private Camera _mainCam;
        private Vector3 _spawnPoint;

        private bool _isPressed = false;

        private CancellationTokenSource _cts;

        public GameFlow(IInputService inputs, IEntitySpawnService spawnService, GameConfig config)
        {
            _inputService = inputs;
            _entitySpawnService = spawnService;
            _gameConfig = config;

            _inputService.OnPress += OnPress;
            _inputService.OnDrag += MoveCurrentEntity;
            _inputService.OnRelease += LaunchCurrentEntity;
            _mainCam = Camera.main;
        }

        public void SetDefaultSpawnPoint(Vector3 spawnPoint)
        {
            _spawnPoint = spawnPoint;
        }

        public void StartGame()
        {
            _cts = new CancellationTokenSource();
            Run(_cts.Token).Forget();
        }

        private void SpawnNewEntity(Vector3 pos)
        {
            var obj = _entitySpawnService.Spawn(pos);
            //animate obj
            //rise on object created signal
            _currentObject = obj;
        }

        private async UniTaskVoid Run(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                SpawnNewEntity(_spawnPoint);
                await WaitUntilLaunched(token);
                await UniTask.Delay(800, cancellationToken: token);
            }
        }

        private async UniTask WaitUntilLaunched(CancellationToken token)
        {
            await UniTask.WaitUntil(() => _currentObject == null, cancellationToken: token);
        }

        private void OnPress()
        {
            if (_currentObject == null)
            {
                _isPressed = false;
                return;
            }

            _isPressed = true;
        }

        private void MoveCurrentEntity(Vector2 delta)
        {
            if (_currentObject == null || !_isPressed)
                return;

            Vector3 currentPos = _currentObject.transform.position;

            Vector3 screenPos = _mainCam.WorldToScreenPoint(currentPos);
            screenPos.x += delta.x * _gameConfig.Sensitivity;

            Vector3 newPos = _mainCam.ScreenToWorldPoint(screenPos);
            newPos.y = currentPos.y;
            newPos.z = currentPos.z;
            newPos.x = Mathf.Clamp(newPos.x, -_gameConfig.ClampX, _gameConfig.ClampX);

            _currentObject.Move(newPos);
        }

        private void LaunchCurrentEntity()
        {
            if (_currentObject == null)
            {
                return;
            }

            _currentObject.Launch(Vector3.forward * _gameConfig.LaunchForce);
            _currentObject = null;
            _isPressed = false;
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