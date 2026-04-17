using Game.Systems.VFX;
using UnityEngine;

namespace Game.Signals
{
    public struct CreateVFXSignal
    {
        public VFXType VFXType { get; private set; }
        public Vector3 Position { get; private set; }

        public CreateVFXSignal(VFXType type, Vector3 position)
        {
            VFXType = type;
            Position = position;
        }
    }
}