using UnityEngine;

namespace Game.Signals
{
    public struct MergeSignal
    {
        public Vector3 Position;
        public int Value;

        public MergeSignal(Vector3 pos, int value)
        {
            Position = pos;
            Value = value;
        }
    }
}