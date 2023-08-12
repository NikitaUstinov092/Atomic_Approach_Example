namespace Lessons.Gameplay.Atomic2
{
    public sealed class HitPointsComponent : IHitPointsComponent
    {
        public int HitPoints
        {
            get { return this.hitPoints.Value; }
        }

        private readonly IAtomicValue<int> hitPoints;

        public HitPointsComponent(IAtomicValue<int> hitPoints)
        {
            this.hitPoints = hitPoints;
        }
    }
}