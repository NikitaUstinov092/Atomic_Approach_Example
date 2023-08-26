using Lessons.Gameplay;

namespace Atomic.Components
{
    public interface IHitPointsComponent
    {
        int HitPoints { get; }
    }

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