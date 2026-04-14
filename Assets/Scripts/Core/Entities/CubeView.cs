using TMPro;
using UnityEngine;

namespace Game.Core
{
    [RequireComponent(typeof(Rigidbody))]
    public class CubeView : EntityView
    {
        [SerializeField] private Rigidbody _rb;
        [SerializeField] private TextMeshPro[] _valueRepresentation;

        private EntityModel _model;


        public override void Init(EntityModel model)
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

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.TryGetComponent(out CubeView other))
            {
                float impulse = collision.impulse.magnitude;
                //MergeHandler try merge
            }
        }
    }
}