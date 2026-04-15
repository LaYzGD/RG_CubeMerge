using Game.Signals;

namespace Game.Features
{
    public interface IMergeService
    {
        public abstract void TryMerge(EntitiesCollisionSignal signal);
    }
}