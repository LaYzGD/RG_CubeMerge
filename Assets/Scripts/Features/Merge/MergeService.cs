using System;
using Game.Signals;
using UnityEngine;

namespace Game.Features
{
    public class MergeService : IMergeService, IDisposable
    {
        private SignalBus _bus;
        private float _impulseThreshold;
        
        public MergeService(SignalBus bus, float impulse)
        {
            _bus = bus;
            _bus.Subscribe<EntitiesCollisionSignal>(TryMerge);
            _impulseThreshold = impulse;
        }

        public void Dispose()
        {
            _bus.Unsubscribe<EntitiesCollisionSignal>(TryMerge);
        }

        public void TryMerge(EntitiesCollisionSignal signal)
        {
            var entityA = signal.A;
            var entityB = signal.B;
            float impulse = signal.Impulse;

            if (entityA == null || entityB == null) return;

            if (!entityA.Model.CanMerge(entityB.Model) || impulse < _impulseThreshold)
            {
                return;
            }

            Vector3 mergeResultPos = entityB.transform.position;
            var mergeResultValue = entityB.Model.MergeResult();

            entityA.Release();
            entityB.Release();

            _bus.Invoke(new CreateMergedEntitySignal(mergeResultPos, mergeResultValue));
        }
    }
}