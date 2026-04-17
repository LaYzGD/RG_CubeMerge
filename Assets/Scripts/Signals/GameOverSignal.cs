namespace Game.Signals 
{
    public struct GameOverSignal 
    {
        public int FinalScore { get; private set; }

        public GameOverSignal(int score) 
        {
            FinalScore = score;
        }
    }
}