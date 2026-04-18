using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

namespace Game.Systems.Input
{
    public class InputService : MonoBehaviour, IInputService
    {
        public event Action OnPress;
        public event Action OnRelease;
        public event Action<Vector2> OnDrag;

        private bool _pressed;
        private Vector2 _lastDelta;
        private bool _releaseRequested;

        public void CheckPress(InputAction.CallbackContext ctx)
        {
            if (ctx.started)
            {
                _pressed = true;
            }

            if (ctx.canceled)
            {
                _releaseRequested = true;
                _pressed = false;
            }
        }

        public void CheckDelta(InputAction.CallbackContext ctx)
        {
            if (!ctx.performed)
                return;

            _lastDelta = ctx.ReadValue<Vector2>();
        }

        private void Update()
        {
            if (IsPointerOverUI())
            {
                ResetFrameInput();
                return;
            }

            if (_pressed)
            {
                OnPress?.Invoke();
                _pressed = false;
            }

            if (_lastDelta != Vector2.zero)
            {
                OnDrag?.Invoke(_lastDelta);
                _lastDelta = Vector2.zero;
            }

            if (_releaseRequested)
            {
                OnRelease?.Invoke();
                _releaseRequested = false;
            }
        }

        private void ResetFrameInput()
        {
            _pressed = false;
            _lastDelta = Vector2.zero;
            _releaseRequested = false;
        }

        private bool IsPointerOverUI()
        {
            if (EventSystem.current == null)
                return false;

            if (Pointer.current != null)
            {
                return EventSystem.current.IsPointerOverGameObject(Pointer.current.deviceId);
            }

            return false;
        }
    }
}