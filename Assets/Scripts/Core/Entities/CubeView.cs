using TMPro;
using UnityEngine;
using System;
using Game.Signals;
using Zenject;
using Game.Configs;

namespace Game.Core
{
    [RequireComponent(typeof(Rigidbody))]
    public class CubeView : EntityView, Infrastructure.IPoolable<CubeView>
    {
        [SerializeField] private Rigidbody _rb;
        [SerializeField] private Collider _collider;
        [SerializeField] private MeshRenderer _meshRenderer; 
        [SerializeField] private TextMeshPro[] _valueRepresentation;
        [SerializeField] private ParticleSystem _trail;

        private IMergeable _model;
        private Action<CubeView> _releaseAction;

        private SignalBus _bus;
        private EntityStatesConfig _statesConfig;
        private EntityFSM _fsm;
        private EntityStateContext _stateContext;
        public override IMergeable Model => _model;
        public override EntityFSM EntityFSM => _fsm;


        [Inject]
        public void Construct(SignalBus bus, GameConfig config)
        {
            _bus = bus;
            _statesConfig = config.EntityStatesConfig;
        }

        public override void Init(IMergeable model)
        {
            _model = model;
            if (_rb == null)
            {
                _rb = GetComponent<Rigidbody>();
            }

            _fsm = new EntityFSM();
            _stateContext = new EntityStateContext(_rb, _statesConfig, _trail);
        }

        public override void Launch(Vector3 force)
        {
            _rb.linearVelocity = force;
            _trail.Play();
            _fsm.ChangeState(new EntityMovingState(_fsm, _stateContext));
        }

        public override void SetNewValue(EntityData data)
        {
            if (_valueRepresentation == null || _valueRepresentation.Length == 0)
            {
                return;
            }

            _meshRenderer.material.color = data.Color;
            foreach (var item in _valueRepresentation) 
            {
                item.text = $"{data.Value}";
            }
        }

        public void ResetState()
        {
            SetKinematic(false);
            _trail.Stop();
            _fsm.ChangeState(new EntityDraggingState(_fsm, _stateContext));
            _rb.rotation = Quaternion.Euler(0, 0, 0);
            _rb.freezeRotation = true;
        }

        public override void SetKinematic(bool flag)
        {
            _rb.isKinematic = flag;
            _collider.enabled = !flag;
        }

        public override void SetMerged()
        {
            _fsm.ChangeState(new EntityMergedState(_fsm, _stateContext));
        }

        public void BindRelease(Action<CubeView> releaseAction) 
        {
            _releaseAction = releaseAction;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (_model.IsMerging)
            {
                return;
            }

            if (collision.gameObject.TryGetComponent(out CubeView other))
            {
                if (other.Model.IsMerging)
                {
                    return;
                }

                other.Model.IsMerging = true;
                Model.IsMerging = true;
                float impulse = collision.impulse.magnitude;
                _bus.Invoke(new EntitiesCollisionSignal(this, other, impulse));
            }
        }

        public override void Release()
        {
            _bus.Invoke(new EntityReleasedSignal(this));
            _releaseAction?.Invoke(this);
        }

        public override void Move(Vector3 pos)
        {
            _rb.MovePosition(pos);
        }

        private void Update()
        {
            _fsm?.Update();
        }

        private void FixedUpdate()
        {
            _fsm?.FixedUpdate();
        }

        public override void SetAutoMerging()
        {
            _fsm.ChangeState(new EntityAutoMergingState(_fsm, _stateContext));
        }
    }
}