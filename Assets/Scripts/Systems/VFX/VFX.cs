using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Infrastructure;
using UnityEngine;

namespace Game.Systems.VFX
{
    public class VFX : MonoBehaviour, IPoolable<VFX>
    {
        [field: SerializeField] public VFXType VFXType { get; private set; }
        [SerializeField] private ParticleSystem[] _particles;

        private CancellationTokenSource _cts;

        private Action<VFX> _releaseAction;

        public void BindRelease(Action<VFX> releaseAction)
        {
            _releaseAction = releaseAction;
        }

        public void PlayVFX()
        {
            _cts = new CancellationTokenSource();
            foreach (var particle in _particles)
            {
                particle.Play();
            }
            ReleaseVFX(_cts.Token).Forget();
        }

        private async UniTask ReleaseVFX(CancellationToken token)
        {
            await UniTask.WaitUntil(AreParticlesDead, cancellationToken: token);
            _releaseAction?.Invoke(this);
        }

        private bool AreParticlesDead()
        {
            foreach (var particle in _particles)
            {
                if (particle.IsAlive())
                {
                    return false;
                }
            }

            return true;
        }

        private void OnDestroy()
        {
            _cts.Cancel();
            _cts.Dispose();
        }
    }
}