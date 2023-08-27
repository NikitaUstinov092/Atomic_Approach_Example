using System;
using Declarative;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace Atomic.GamePlay.Scripts.Zombie
{
    [Serializable]
    public class ZombieModel_View
    {
        private static readonly int State = Animator.StringToHash("State");
        private const int MOVE_STATE = 1;
        private const int ATTACK_STATE = 3;
        private const int DEATH_STATE = 5;
        
        [SerializeField]
        public Animator animator;

        [SerializeField]
        public Transform visualTransform;
        
        private readonly LateUpdateMechanics lateUpdate = new();

        [Construct]
        public void Construct(ZombieModel_Core core)
        {
            var isDeath = core.life.isDeath;
            var isChasing = core.ZombieChase.IsChasing;
            var chasingObject = core.ZombieChase.Target.Value;
            
            lateUpdate.Construct(_ =>
            {
                if (isDeath.Value)
                {
                    animator.SetInteger(State, DEATH_STATE);
                    return;
                }
                if (isChasing.Value)
                {
                    animator.SetInteger(State, MOVE_STATE);
                    visualTransform.LookAt(chasingObject.position);
                }
                else if (!isChasing.Value)
                {
                    animator.SetInteger(State, ATTACK_STATE);
                    visualTransform.LookAt(chasingObject.position);
                }
            });
        }
    }
}
