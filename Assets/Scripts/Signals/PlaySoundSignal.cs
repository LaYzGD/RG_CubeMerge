using Game.Systems.Audio;
namespace Game.Signals
{
    public struct PlaySoundSignal 
    {
        public SoundType Type { get; private set; }

        public PlaySoundSignal(SoundType type)
        {
            Type = type;
        }
    }
}