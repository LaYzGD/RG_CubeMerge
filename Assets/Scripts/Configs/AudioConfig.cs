using Game.Systems.Audio;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Configs
{
    [CreateAssetMenu(menuName = "Configs/AudioConfig", fileName = "AudioConfig")]
    public class AudioConfig : ScriptableObject
    {
        [field: SerializeField] public List<AudioEntry> AudioEntries { get; private set; }
    }

    [System.Serializable]
    public struct AudioEntry
    {
        public SoundType SoundType;
        public AudioClip AudioClip;
        public float MinPitch;
        public float MaxPitch;
    }
}