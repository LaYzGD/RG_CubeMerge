using System.Collections.Generic;
using Game.Features.Spawn;
using UnityEngine;

namespace Game.Configs
{
    [CreateAssetMenu(menuName = "Configs/SpawnConfig", fileName = "New SpawnConfig")]
    public class SpawnConfig : ScriptableObject
    {
        [SerializeField]
        private SpawnData[] _spawnData;

        public IReadOnlyList<SpawnData> SpawnDatas => _spawnData;
    }
}