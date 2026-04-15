using UnityEngine;

namespace Game.Signals
{
    public struct CreateMergedEntitySignal
    {
        public Vector3 Position;
        public int Value;

        public CreateMergedEntitySignal(Vector3 pos, int value)
        {
            Position = pos;
            Value = value;
        }
    }
}