namespace Game.Core
{
    public interface IMergeable
    {
        public float ForceAfterMerge { get; }
        public bool IsMerging { get; set; }
        public int Value { get; }
        public bool CanMerge(IMergeable other);
        public int MergeResult();
    }
}