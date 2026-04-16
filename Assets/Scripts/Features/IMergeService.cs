using Game.Core;
using Game.Signals;

namespace Game.Features
{
    public interface IMergeService
    {
        public abstract void TryMerge(EntitiesCollisionSignal signal);
        public abstract void Merge(EntityView a, EntityView b);
    }
}