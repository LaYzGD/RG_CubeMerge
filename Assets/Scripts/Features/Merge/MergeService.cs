using System;
using DG.Tweening;
using Game.Core;
using Game.Signals;
using UnityEngine;

namespace Game.Features.Merge
{
    public class MergeService : IMergeService, IDisposable
    {
        private SignalBus _bus;
        private float _impulseThreshold;
        private IEntitiesRegistry _entitiesRegistry;
        
        public MergeService(SignalBus bus, IEntitiesRegistry registry, float impulse)
        {
            _bus = bus;
            _entitiesRegistry = registry;
            _bus.Subscribe<EntitiesCollisionSignal>(TryMerge);
            _impulseThreshold = impulse;
        }

        public void Dispose()
        {
            _bus.Unsubscribe<EntitiesCollisionSignal>(TryMerge);
        }

        public void Merge(EntityView entityA, EntityView entityB)
        {
            Vector3 mergeResultPos = entityB.transform.position;
            var mergeResultValue = entityB.Model.MergeResult();

            _entitiesRegistry.Unregister(entityA);
            _entitiesRegistry.Unregister(entityB);

            entityA.transform.DOKill();
            entityB.transform.DOKill();

            entityA.Release();
            entityB.Release();

            _bus.Invoke(new MergeSignal(mergeResultPos, mergeResultValue));
        }

        public void TryMerge(EntitiesCollisionSignal signal)
        {
            var entityA = signal.A;
            var entityB = signal.B;
            float impulse = signal.Impulse;
            Vector3 particlesPos = entityB.transform.position;

            if (entityA == null || entityB == null) return;

            if (!entityA.Model.CanMerge(entityB.Model) || impulse < _impulseThreshold)
            {
                entityA.Model.IsMerging = false;
                entityB.Model.IsMerging = false;
                return;
            }

            Merge(entityA, entityB);
            _bus.Invoke(new CreateVFXSignal(Systems.VFX.VFXType.Merge, particlesPos));
            _bus.Invoke(new PlaySoundSignal(Systems.Audio.SoundType.Merge));
        }
    }
}