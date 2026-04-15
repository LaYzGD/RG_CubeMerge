namespace Game.Core
{
    public interface IMergeable
    {
        public int Value { get; }
        public bool CanMerge(IMergeable other);
        public int MergeResult();
    }
}