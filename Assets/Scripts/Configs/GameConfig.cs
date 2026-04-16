using Game.Core;
using UnityEngine;

namespace Game.Configs
{
    [CreateAssetMenu(menuName = "Configs/GameConfig", fileName = "GameConfig")]
    public class GameConfig : ScriptableObject
    {
        [field: SerializeField] public float Sensitivity { get; private set; } = 1.2f;
        [field: SerializeField] public float ClampX { get; private set; } = 2f;
        [field: SerializeField] public float LaunchForce { get; private set; } = 20f;
        [field: SerializeField] public float ImpulseThreshold { get; private set; } = 2f;
        [field: SerializeField] public CubeView CubeViewPrefab { get; private set; }
        [field: SerializeField] public SpawnConfig SpawnConfig { get; private set; }
        [field: SerializeField] public EntityConfig EntityConfig { get; private set; }
        [field: SerializeField] public AutoMergeConfig AutoMergeConfig { get; private set; }
    }
}
