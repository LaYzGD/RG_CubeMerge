using System;

namespace Game.Infrastructure
{
    public interface IPoolable<T>
    {
        public abstract void BindRelease(Action<T> releaseAction);
    }
}