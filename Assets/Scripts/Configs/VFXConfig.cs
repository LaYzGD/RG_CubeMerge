using System.Collections.Generic;
using Game.Systems.VFX;
using UnityEngine;

namespace Game.Configs
{
    [CreateAssetMenu(menuName = "Configs/VFXConfig", fileName = "VFXConfig")]
    public class VFXConfig : ScriptableObject
    {
        [SerializeField] private List<VFXEntry> _vfxEntries;

        public IReadOnlyList<VFXEntry> VfxEntries => _vfxEntries;
    }

    [System.Serializable]
    public struct VFXEntry
    {
        public VFXType Type;
        public VFX VFX;
    }
}