using Game.Core;

namespace Game.Signals
{
    public struct RegisterEntitySignal
    {
        public EntityView Entity { get; private set; }

        public RegisterEntitySignal(EntityView entity)
        {
            Entity = entity;
        }
    }
}