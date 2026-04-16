using System;
using UnityEngine;

namespace Game.Systems.Input
{
    public interface IInputService
    {
       public event Action OnPress;
       public event Action OnRelease;
       public event Action<Vector2> OnDrag;
    }
}