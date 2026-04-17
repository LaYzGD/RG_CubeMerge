using Game.Configs;
using UnityEngine;

namespace Game.Core
{
    public struct EntityStateContext
    {
        public Rigidbody Rigidbody { get; private set; }
        public EntityStatesConfig Config { get; private set; }
        public ParticleSystem Trail { get; private set; }

        public EntityStateContext(Rigidbody rb, EntityStatesConfig config, ParticleSystem trail)
        {
            Rigidbody = rb;
            Config = config;
            Trail = trail;
        }
    }
}