using Game.Core;
using UnityEngine;

namespace Game.Configs
{
    [CreateAssetMenu(menuName = "Configs/GameConfig", fileName = "GameConfig")]
    public class GameConfig : ScriptableObject
    {
        [field: SerializeField] public EntityView EntityViewPrefab { get; private set; }
        [field: SerializeField] public SpawnConfig SpawnConfig { get; private set; }
        [field: SerializeField] public EntityConfig EntityConfig { get; private set; }
    }
}
