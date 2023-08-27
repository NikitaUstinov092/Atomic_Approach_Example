using System;
using Declarative;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace Lessons.Gameplay.Atomic1
{
    [Serializable]
    public sealed class HeroModel_View
    {
        private static readonly int State = Animator.StringToHash("State");
        private const int IDLE_STATE = 0;
        private const int MOVE_STATE = 1;
        private const int ATTACK_STATE = 3;
        private const int DEATH_STATE = 5;

        [SerializeField]
        public Animator animator;

        [SerializeField]
        public Transform visualTransform;
        
        public Vector3 rotationAngle;

        private readonly LateUpdateMechanics lateUpdate = new();

        [Construct]
        public void Construct(HeroModel_Core core)
        {
            var isDeath = core.life.isDeath;
            var moveRequired = core.move.moveRequired;
            var fireEvent = core.shoot.OnGetPressedFire;
            
            this.lateUpdate.Construct(_ =>
            {
                if (isDeath.Value)
                {
                    animator.SetInteger(State, DEATH_STATE);
                    return;
                }

                if (moveRequired.Value)
                {
                    animator.SetInteger(State, MOVE_STATE);
                    return;
                }
               
                animator.SetInteger(State, IDLE_STATE);
                
                
                fireEvent += () =>
                {
                    if(moveRequired.Value)
                        return;
                    animator.SetInteger(State, ATTACK_STATE);
                };
                
            });
        }
    }
}