using System;
using Assets.Scripts.Custom;
using Declarative;
using Lessons.Gameplay;
using ScriptableObject;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace GamePlay.Scripts
{
    [Serializable]
    public sealed class HeroModel_Core
    {
        [Section]
        [SerializeField]
        public Life life = new();

        [Section]
        [SerializeField]
        public Move move = new();
        
        [Section]
        [SerializeField]
        public Rotate rotate = new();
        
        [Section]
        [SerializeField]
        public Shoot shoot = new();

        [Serializable]
        public sealed class Life
        {
            [ShowInInspector]
            public AtomicEvent<int> onTakeDamage = new();

            [SerializeField]
            public AtomicVariable<int> hitPoints = new();

            [SerializeField]
            public AtomicVariable<bool> isDeath;

            [Construct]
            public void Construct()
            {
                this.onTakeDamage += damage => this.hitPoints.Value -= damage;
                this.hitPoints.OnChanged += hitPoints =>
                {
                    if (hitPoints <= 0) this.isDeath.Value = true;
                };
            }
        }

        [Serializable]
        public sealed class Move
        {
            [SerializeField]
            public Transform moveTransform;
          
            [ShowInInspector]
            public AtomicEvent<Vector3> onMove = new();

            [SerializeField]
            public AtomicVariable<bool> moveRequired = new();

            [SerializeField]
            public AtomicVariable<Vector3> moveDirection = new();

            [SerializeField]
            public AtomicVariable<float> speed = new();

            private readonly FixedUpdateMechanics fixedUpdate = new();

            [Construct]
            public void Construct(Life life)
            {
                var isDeath = life.isDeath;
                onMove += direction =>
                {
                    if (isDeath.Value)
                    {
                        return;
                    }
                    moveDirection.Value = (moveTransform.forward * direction.z + moveTransform.right * direction.x).normalized;
                    moveRequired.Value = true;
                };

                fixedUpdate.Construct(deltaTime =>
                {
                    if (moveRequired.Value)
                    {
                        moveTransform.position += this.moveDirection.Value * (this.speed.Value * deltaTime);
                        moveRequired.Value = false;
                    }
                });
            }
        }
        [Serializable]
        public sealed class Rotate
        { 
            [SerializeField]
            public Camera PlayerCamera;
            
            [SerializeField]
            public Transform PlayerTransform;
            
            public RotationEngine RotationMotor = new();
            
            public AtomicEvent<Vector3> OnGetVectorCursor = new();
           
            private readonly FixedUpdateMechanics fixedUpdate = new();

            [Construct]
            public void Construct(Life life)
            {
                var isDeath = life.isDeath;

                RotationMotor.Construct(PlayerTransform, PlayerCamera);

                OnGetVectorCursor += rotateDir =>
                {
                    if (isDeath.Value)
                        return;

                    RotationMotor.SetRotationVector(rotateDir);
                };

                fixedUpdate.Construct(deltaTime =>
                {
                    RotationMotor.UpdateRotation();
                });

            }
        }

        [Serializable]
        public sealed class Shoot
        {
            public ShootEngine ShootEngine;
            public BulletConfig BulletConfig;
            public AtomicEvent OnGetPressedFire = new();
            /*private readonly FixedUpdateMechanics fixedUpdate = new();*/

            [Construct]
            public void Construct(Life life)
            {
                ShootEngine.Construct((BulletConfig));
                OnGetPressedFire += () => { ShootEngine.CreateBullet(); };
                
                /*fixedUpdate.Construct(deltaTime =>
                {
                    ShootEngine.Cooldown();
                });*/
            }

        }
    }
}