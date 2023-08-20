using System;
using System.Numerics;
using Declarative;
using GamePlay.Scripts;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

namespace Lessons.Gameplay.Atomic1
{
    [Serializable]
    public sealed class HeroModel_View
    {
        private static readonly int State = Animator.StringToHash("State");
        private const int IDLE_STATE = 0;
        private const int MOVE_STATE = 1;
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
            var moveDirection = core.move.moveDirection;
            
            this.lateUpdate.Construct(_ =>
            {
                if (isDeath.Value)
                {
                    this.animator.SetInteger(State, DEATH_STATE);
                    return;
                }

                if (moveRequired.Value)
                {
                    this.animator.SetInteger(State, MOVE_STATE);
                }
                else
                {
                    this.animator.SetInteger(State, IDLE_STATE);
                }
                
            });
        }
    }
}