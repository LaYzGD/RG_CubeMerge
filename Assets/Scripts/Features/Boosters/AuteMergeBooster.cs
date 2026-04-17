using Cysharp.Threading.Tasks;
using DG.Tweening;
using Game.Configs;
using Game.Core;
using Game.Features.Merge;
using Game.Signals;
using UnityEngine;

namespace Game.Features.Boosters
{
    public class AutoMergeBooster : IBooster
    {
        private IEntitiesRegistry _entitiesRegistry;
        private IMergeService _mergeService;
        private AutoMergeConfig _config;
        private SignalBus _bus;

        private const int _objectsToMergeAmount = 2;

        public AutoMergeBooster(SignalBus bus, IEntitiesRegistry registry, IMergeService mergeService, GameConfig config)
        {
            _bus = bus;
            _entitiesRegistry = registry;
            _mergeService = mergeService;
            _config = config.AutoMergeConfig;
        }

        public async UniTask Execute()
        {
            var awailableValues = _entitiesRegistry.GetRegisteredValues(_objectsToMergeAmount);
            if (awailableValues == null || awailableValues.Count == 0)
            {
                return;
            }

            int randomValue = awailableValues[UnityEngine.Random.Range(0, awailableValues.Count)];

            var registeredEntities = _entitiesRegistry.GetEntitiesWithValue(randomValue);
            if (registeredEntities == null) 
            {
                return;
            }

            var entityA = registeredEntities[0];
            var entityB = registeredEntities[1];

            if (entityA.Model.IsMerging || entityB.Model.IsMerging)
            {
                return;
            }

            entityA.Model.IsMerging = true;
            entityB.Model.IsMerging = true;

            entityA.SetAutoMerging();
            entityB.SetAutoMerging();

            await AnimateLift(entityA, entityB);
            await AnimateSwing(entityA, entityB);
            await AnimateMerge(entityA, entityB);

            _mergeService.Merge(entityA, entityB);

            await UniTask.Delay(1000);
        }

        private async UniTask AnimateLift(EntityView entityA, EntityView entityB)
        {
            entityA.SetKinematic(true);
            entityB.SetKinematic(true);

            float height = _config.LiftHeight;
            float duration = _config.LiftDuration;

            Sequence seq = DOTween.Sequence();

            seq.Join(entityA.transform.DOMoveY(entityA.transform.position.y + height, duration)
                .SetEase(Ease.OutQuad));

            seq.Join(entityB.transform.DOMoveY(entityB.transform.position.y + height, duration)
                .SetEase(Ease.OutQuad));

            await seq.AsyncWaitForCompletion().AsUniTask();
        }

        private async UniTask AnimateSwing(EntityView entityA, EntityView entityB)
        {
            float duration = _config.SwingDuration;
            float swingAmplitude = _config.SwingAmplitude;

            Vector3 dir = (entityB.transform.position - entityA.transform.position).normalized;

            Sequence seq = DOTween.Sequence();

            seq.Join(entityA.transform.DOMove(entityA.transform.position - dir * swingAmplitude, duration)
                .SetEase(Ease.OutBack));

            seq.Join(entityB.transform.DOMove(entityB.transform.position + dir * swingAmplitude, duration)
                .SetEase(Ease.OutBack));

            await seq.AsyncWaitForCompletion().AsUniTask();
        }

        private async UniTask AnimateMerge(EntityView entityA, EntityView entityB)
        {
            float duration = _config.MergeDuration;

            Vector3 center = (entityA.transform.position + entityB.transform.position) / 2;

            Sequence seq = DOTween.Sequence();

            seq.Join(entityA.transform.DOMove(center, duration)
                .SetEase(Ease.InBack));

            seq.Join(entityB.transform.DOMove(center, duration)
                .SetEase(Ease.InBack));

            await seq.AsyncWaitForCompletion().AsUniTask();
            _bus.Invoke(new CreateVFXSignal(Systems.VFX.VFXType.AutoMerge, center));
        }
    }
}