using Game.Core;

namespace Game.Signals
{
    public struct EntitiesCollisionSignal
    {
        public EntityView A;
        public EntityView B;
        public float Impulse;

        public EntitiesCollisionSignal(EntityView A, EntityView B, float Impulse)
        {
            this.A = A;
            this.B = B;
            this.Impulse = Impulse;
        }
    }
}