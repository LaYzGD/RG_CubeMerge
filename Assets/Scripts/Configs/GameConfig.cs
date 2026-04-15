using Game.Core;
using UnityEngine;

namespace Game.Configs
{
    [CreateAssetMenu(menuName = "Configs/GameConfig", fileName = "GameConfig")]
    public class GameConfig : ScriptableObject
    {
        [field: SerializeField] public float ImpulseThreshold { get; private set; } = 2f;
        [field: SerializeField] public CubeView CubeViewPrefab { get; private set; }
        [field: SerializeField] public SpawnConfig SpawnConfig { get; private set; }
        [field: SerializeField] public EntityConfig EntityConfig { get; private set; }
    }
}
