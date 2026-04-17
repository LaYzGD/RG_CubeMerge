using Game.Core;

namespace Game.Signals 
{
    public struct EntityReleasedSignal
    {
        public EntityView Entity { get; private set; }

        public EntityReleasedSignal(EntityView entity) {  Entity = entity; }
    }
}