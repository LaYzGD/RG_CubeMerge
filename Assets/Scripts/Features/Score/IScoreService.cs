using System;

namespace Game.Features.Score
{
    public interface IScoreService
    {
        public int CurrentScore { get; }
        public void Add(int value);
        public event Action<int> OnScoreChanged;
    }
}