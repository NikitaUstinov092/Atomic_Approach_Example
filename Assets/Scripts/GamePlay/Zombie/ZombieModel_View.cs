using System;
using System.Declarative.Scripts.Attributes;
using UnityEngine;
using UpdateMechanics;

namespace GamePlay.Zombie
{
    [Serializable]
    public class ZombieModel_View
    {
        private static readonly int State = Animator.StringToHash("State");
        
        private const int MOVE_STATE = 1;
        private const int IDLE_STATE = 2;
        private const int ATTACK_STATE = 3;
        private const int DEATH_STATE = 4;
        
        [SerializeField]
        public Animator animator;
        
        private readonly LateUpdateMechanics lateUpdate = new();

        [Construct]
        public void Construct(ZombieModel_Core core)
        {
            var isDeath = core.life.IsDead;
            var isChasing = core.ZombieChase.IsChasing;
            var stopAttack = core.AttackHero.StopAttack;
            
            lateUpdate.Construct(_ =>
            {
                if (isDeath.Value)
                {
                    animator.SetInteger(State, DEATH_STATE);
                    return;
                }

                if (stopAttack.Value)
                {
                    animator.SetInteger(State, IDLE_STATE);
                    return;
                }
                
                switch (isChasing.Value)
                {
                    case true:
                        animator.SetInteger(State, MOVE_STATE);
                        break;
                    
                    case false:
                        animator.SetInteger(State, ATTACK_STATE);
                        break;
                }
            });
        }
    }
}

