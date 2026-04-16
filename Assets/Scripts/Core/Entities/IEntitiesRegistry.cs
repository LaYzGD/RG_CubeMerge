using System.Collections.Generic;

namespace Game.Core
{
    public interface IEntitiesRegistry
    {
        public void Register(EntityView view);
        public void Unregister(EntityView view);
        public IReadOnlyList<EntityView> GetEntitiesWithValue(int value);
        public IReadOnlyList<int> GetRegisteredValues(int entitiesAmount);
    }
}