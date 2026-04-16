using System;
using Game.Signals;

namespace Game.Features.Score
{
    public class ScoreService : IScoreService
    {
        private int _score;
        public int CurrentScore => _score;

        public event Action<int> OnScoreChanged;

        private SignalBus _bus;

        public ScoreService(SignalBus bus)
        {
            _bus = bus;

            _bus.Subscribe<MergeSignal>(HandleMergeSignal);
        }

        private void HandleMergeSignal(MergeSignal signal) 
        {
            int reward = signal.Value / 2;
            Add(reward);
        }

        public void Add(int value)
        {
            if (value <= 0)
            {
                return;
            }

            _score += value;
            OnScoreChanged?.Invoke(_score);
        }
    }
}