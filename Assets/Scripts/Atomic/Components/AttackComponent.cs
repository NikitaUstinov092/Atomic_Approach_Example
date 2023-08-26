using Lessons.Gameplay;
using UnityEngine;

namespace Atomic.Components
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