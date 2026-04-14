using UnityEngine;

namespace Game.Core
{
    [System.Serializable]
    public struct EntityData
    {
        public int Value;
        public Color Color;

        public EntityData(int value, Color color)
        {
            Value = value;
            Color = color;
        }
    }
}