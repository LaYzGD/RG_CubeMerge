using UnityEngine;

namespace Game.Configs
{
    [CreateAssetMenu(menuName = "Configs/EntityStatesConfig", fileName = "EntityStatesConfig")]
    public class EntityStatesConfig : ScriptableObject
    {
        [field: SerializeField] public float MovementThreshold { get; private set; } = 0.1f;
        [field: SerializeField] public float MergedForce { get; private set; } = 140f;
    }
}