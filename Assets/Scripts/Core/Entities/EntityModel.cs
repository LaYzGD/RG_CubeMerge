namespace Game.Core
{
    public class EntityModel : IMergeable
    {
        public int Value { get; private set; }

        public EntityModel(int value)
        {
            Value = value;
        }

        public bool CanMerge(IMergeable other, float impulse)
        {
            if (other is not EntityModel e)
            {
                return false;
            }

            //change magic number to config.impulse
            return e.Value == Value && impulse >= 2f;
        }

        public int MergeResult()
        {
            return Value * 2;
        }
    }
}