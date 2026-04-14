namespace Game.Core
{
    public interface IMergeable
    {
        public int Value { get; }
        public bool CanMerge(IMergeable other, float impulse);
        public int MergeResult();
    }
}