using System;
using System.Atomic.Implementations;
using System.Declarative.Scripts.Attributes;
using TMPro;
using UnityEngine;
using UpdateMechanics;
using Vector3 = UnityEngine.Vector3;


    namespace GamePlay.Hero
    {
        [Serializable]
        public sealed class HeroModel_View
        {
            [Section]
            [SerializeField]
            public HeroAnimation_View HeroAnimationView = new();
        
            [Section]
            [SerializeField]
            public HP_View HpView = new();

            [Serializable]
            public sealed class HeroAnimation_View
            {
                private static readonly int State = Animator.StringToHash("State");
                private const int IDLE_STATE = 0;
                private const int MOVE_STATE_FRONT = 1;
                private const int MOVE_STATE_RIGHT = 2;
                private const int MOVE_STATE_LEFT = 3;
                private const int MOVE_STATE_BACK = 4;
                private const int DEATH_STATE = 6;
            
                [SerializeField]
                public Animator animator;
            
                private readonly LateUpdateMechanics lateUpdate = new();
            
                [Construct]
                public void Construct(HeroModel_Core core)
                {
                    var isDeath = core.life.IsDead;
                    var moveRequired = core.move.moveRequired;
                    var inputVector = core.move.onMove;
            
                    lateUpdate.Construct(_ =>
                    {
                        if (isDeath.Value)
                        {
                            animator.SetInteger(State, DEATH_STATE);
                            return;
                        }
                
                        if (!moveRequired.Value)
                        {
                            animator.SetInteger(State, IDLE_STATE);
                            return;
                        }
                
                        inputVector.Subscribe(direction =>
                        {
                            if (isDeath.Value)
                                return;
                    
                            if (direction == Vector3.forward)
                            {
                                animator.SetInteger(State, MOVE_STATE_FRONT);
                                return;
                            }
                            if (direction == -Vector3.forward)
                            {
                                animator.SetInteger(State, MOVE_STATE_BACK);
                                return;
                            }
                            if (direction == Vector3.left)
                            {
                                animator.SetInteger(State, MOVE_STATE_LEFT);
                                return;
                            }
                    
                            if (direction  == Vector3.right)
                            {
                                animator.SetInteger(State, MOVE_STATE_RIGHT);
                            }
                        });
                    });
                }
            }
        
            [Serializable]
            public sealed class HP_View
            {
                [SerializeField] 
                public AtomicVariable<string> Title;
            
                [SerializeField] 
                public AtomicVariable<TextMeshProUGUI> TextHp;
                public void Construct(HeroModel_Core core)
                {
                    var onTakeDamage = core.life.OnTakeDamage;
                
                }
            }
        }
    }

    
