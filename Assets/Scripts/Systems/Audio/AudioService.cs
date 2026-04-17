using System.Collections.Generic;
using Game.Configs;
using Game.Signals;
using UnityEngine;
using Zenject;

namespace Game.Systems.Audio
{
    public class AudioService : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource;

        private Dictionary<SoundType, AudioEntry> _soundBank;

        private SignalBus _bus;
        private AudioConfig _audioConfig;

        [Inject]
        public void Construct(SignalBus bus, GameConfig config)
        {
            _bus = bus;
            _audioConfig = config.AudioConfig;

            _soundBank = new Dictionary<SoundType, AudioEntry>();
            foreach (var sound in _audioConfig.AudioEntries)
            {
                _soundBank[sound.SoundType] = sound;
            }

            _bus.Subscribe<PlaySoundSignal>(PlaySound);
        }

        private void PlaySound(PlaySoundSignal signal)
        {
            if (!_soundBank.TryGetValue(signal.Type, out var entry))
            {
                return;
            }

            _audioSource.pitch = Random.Range(entry.MinPitch, entry.MaxPitch);
            _audioSource.PlayOneShot(entry.AudioClip);
        }

        private void OnDestroy()
        {
            _bus.Unsubscribe<PlaySoundSignal>(PlaySound);
        }
    }
}
