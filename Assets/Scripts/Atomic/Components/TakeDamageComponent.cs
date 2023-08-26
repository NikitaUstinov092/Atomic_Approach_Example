using Lessons.Gameplay;

namespace Atomic.Components
{
    public interface ITakeDamageComponent
    {
        void TakeDamage(int damage);
    }

    public sealed class TakeDamageComponent : ITakeDamageComponent
    {
        private readonly IAtomicAction<int> onTakeDamage;

        public TakeDamageComponent(IAtomicAction<int> onTakeDamage)
        {
            this.onTakeDamage = onTakeDamage;
        }

        void ITakeDamageComponent.TakeDamage(int damage)
        {
            this.onTakeDamage.Invoke(damage);
        }
    }
}