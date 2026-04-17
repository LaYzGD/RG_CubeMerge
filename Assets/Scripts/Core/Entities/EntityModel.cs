namespace Game.Core
{
    public class EntityModel : IMergeable
    {
        public int Value { get; private set; }
        public bool IsMerging { get; set; }

        private const float _forceAfterMerge = 140f;
        public float ForceAfterMerge => _forceAfterMerge;

        public EntityModel(int value)
        {
            Value = value;
        }

        public bool CanMerge(IMergeable other)
        {
            if (other is not EntityModel)
            {
                return false;
            }

            return other.Value == Value;
        }

        public int MergeResult()
        {
            return Value * 2;
        }
    }
}