using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Systems.Input
{
    public class InputService : MonoBehaviour, IInputService
    {
        public event Action OnPress;
        public event Action OnRelease;
        public event Action<Vector2> OnDrag;

        public void CheckPress(InputAction.CallbackContext ctx)
        {
            if (ctx.started)
            {
                OnPress?.Invoke();
                return;
            }

            if (ctx.canceled)
            {
                OnRelease?.Invoke();
            }
        }

        public void CheckDelta(InputAction.CallbackContext ctx)
        {
            if (ctx.performed)
            {
                Vector2 delta = ctx.ReadValue<Vector2>();
                OnDrag?.Invoke(delta);
            }
        }
    }
}