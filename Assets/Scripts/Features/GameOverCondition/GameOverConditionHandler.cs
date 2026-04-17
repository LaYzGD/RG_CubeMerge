using Game.Core;
using UnityEngine;

namespace Game.Features
{
    public class GameOverConditionHandler : MonoBehaviour, IGameOverService
    {
        [SerializeField] private Transform _center;
        [SerializeField] private Vector3 _size;
        [SerializeField] private LayerMask _mask;

        private Vector3 _halfExtents;

        private void Start ()
        {
            _halfExtents = 0.5f * _size;
        }

        public bool IsGameOver()
        {
            var hits = Physics.OverlapBox(
                _center.position,
                _halfExtents,
                Quaternion.identity,
                _mask
            );

            if (hits == null || hits.Length == 0)
                return false;

            for (int i = 0; i < hits.Length; i++)
            {
                if (!hits[i].TryGetComponent(out EntityView view))
                    continue;

                if (view == null || !view.gameObject.activeInHierarchy)
                    continue;

                if (IsValidForGameOver(view))
                    return true;
            }

            return false;
        }

        private bool IsValidForGameOver(EntityView view)
        {
            var state = view.EntityFSM.CurrentState;

            return state is EntityIdleState;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube( _center.position, _size * 0.5f );
        }
    }
}