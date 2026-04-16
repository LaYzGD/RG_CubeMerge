using System.Collections.Generic;
using System.Linq;
using Game.Signals;

namespace Game.Core
{
    public class EntitiesRegistry : IEntitiesRegistry
    {
        private Dictionary<int, List<EntityView>> _registry;
        private SignalBus _bus;

        public EntitiesRegistry(SignalBus bus)
        {
            _bus = bus;
            _registry = new Dictionary<int, List<EntityView>>();

            _bus.Subscribe<RegisterEntitySignal>(RegisterBySignal);
        }

        public IReadOnlyList<EntityView> GetEntitiesWithValue(int value)
        {
            if (_registry.TryGetValue(value, out var list))
            {
                return list;
            }

            return null;
        }

        public IReadOnlyList<int> GetRegisteredValues(int entitiesAmount)
        {
            var result = new List<int>();

            foreach (var pair in _registry)
            {
                var list = pair.Value;

                if (list != null && list.Count >= entitiesAmount)
                {
                    result.Add(pair.Key);
                }
            }

            return result;
        }

        public void Register(EntityView entity)
        {
            if (entity == null) return;

            int key = entity.Model.Value;

            if (!_registry.TryGetValue(key, out var list))
            {
                list = new List<EntityView>(4);
                _registry.Add(key, list);
            }

            list.Add(entity);
        }

        private void RegisterBySignal(RegisterEntitySignal signal)
        {
            var entity = signal.Entity;

            Register(entity);
        }

        public void Unregister(EntityView view)
        {
            if (view == null) return;

            int key = view.Model.Value;

            if (!_registry.TryGetValue(key, out var list))
                return;

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] == view)
                {
                    int lastIndex = list.Count - 1;
                    list[i] = list[lastIndex];
                    list.RemoveAt(lastIndex);
                    break;
                }
            }

            if (list.Count == 0)
            {
                _registry.Remove(key);
            }
        }
    }
}