using System.Collections.Generic;
using Game.Core;
using UnityEngine;

namespace Game.Configs
{
    [CreateAssetMenu(menuName = "Configs/EntityConfig", fileName = "NewEntityConfig")]
    public class EntityConfig : ScriptableObject
    {
        [SerializeField]
        private EntityData[] _entityDatabase;

        public IReadOnlyList<EntityData> Database => _entityDatabase;
    }
}