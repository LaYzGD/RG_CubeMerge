using TMPro;
using UnityEngine;
using System;
using Zenject;
using Game.Signals;

namespace Game.Core
{
    [RequireComponent(typeof(Rigidbody))]
    public class CubeView : EntityView, Infrastructure.IPoolable<CubeView>
    {
        [SerializeField] private Rigidbody _rb;
        [SerializeField] private TextMeshPro[] _valueRepresentation;

        private IMergeable _model;
        private Action<CubeView> _releaseAction;

        private SignalBus _bus;

        public override IMergeable Model => _model;

        public void SetBus(SignalBus bus)
        {
            _bus = bus;
        }

        public override void Init(IMergeable model)
        {
            _model = model;
            if (_rb == null)
            {
                _rb = GetComponent<Rigidbody>();
            }
        }

        public override void Launch(Vector3 force)
        {
            _rb.AddForce(force, ForceMode.Impulse);
        }

        public override void SetNewValue(EntityData data)
        {
            if (_valueRepresentation == null || _valueRepresentation.Length == 0)
            {
                return;
            }

            foreach (var item in _valueRepresentation) 
            {
                item.color = data.Color;
                item.text = $"{data.Value}";
            }
        }

        public void ResetState()
        {
            _rb.linearVelocity = Vector3.zero;
        }

        public void BindRelease(Action<CubeView> releaseAction) 
        {
            _releaseAction = releaseAction;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.TryGetComponent(out CubeView other))
            {
                float impulse = collision.impulse.magnitude;
                _bus.Invoke(new EntitiesCollisionSignal(this, other, impulse));
            }
        }

        public override void Release()
        {
            _releaseAction?.Invoke(this);
        }
    }
}