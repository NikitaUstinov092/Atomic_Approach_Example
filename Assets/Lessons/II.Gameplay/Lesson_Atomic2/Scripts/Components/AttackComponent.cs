using UnityEngine;

namespace Lessons.Gameplay.Atomic2
{
    public interface IAttackComponent
    {
        void Attack(GameObject target);
    }
    
    public sealed class AttackComponent : IAttackComponent
    {
        private readonly IAtomicAction<GameObject> onAttack;

        public AttackComponent(IAtomicAction<GameObject> onAttack)
        {
            this.onAttack = onAttack;
        }

        void IAttackComponent.Attack(GameObject target)
        {
            this.onAttack.Invoke(target);
        }
    }
}