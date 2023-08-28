using System;
using Assets.Scripts.Custom;
using Declarative;
using Lessons.Gameplay;
using ScriptableObject;
using Sirenix.OdinInspector;
using UnityEngine;


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
            
            [ShowInInspector]
            public AtomicEvent onDeath = new();

            [SerializeField]
            public AtomicVariable<int> hitPoints = new();

            [SerializeField]
            public AtomicVariable<bool> isDeath; //Нахер не нужна

            [Construct]
            public void Construct()
            {
                onTakeDamage += damage =>
                {
                    hitPoints.Value -= damage;
                };
                hitPoints.OnChanged += hitPoints =>
                {
                    if (hitPoints <= 0)
                    {
                        isDeath.Value = true;
                        onDeath?.Invoke();
                    }
                };
            }
        }

        [Serializable]
        public class Move
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
            
            public AtomicVariable<Vector3> RotateDirection;
            
            public AtomicVariable<float> RotationSpeed;
           
            private readonly FixedUpdateMechanics fixedUpdate = new();

            [Construct]
            public void Construct(Life life)
            {
                var isDeath = life.isDeath.Value;

                RotationMotor.Construct(PlayerTransform, PlayerCamera, RotationSpeed.Value);
                
                fixedUpdate.Construct(deltaTime =>
                {
                    if(isDeath)
                        return;
                    
                    var cursorScreenPos = RotateDirection.Value;
                    RotationMotor.UpdateRotation(cursorScreenPos);
                });

            }
        }

        [Serializable]
        public sealed class Shoot
        {
            public ShootEngine ShootEngine;
            public BulletConfig BulletConfig;
            public Transform SpawnPointShoot;
            public AtomicEvent OnGetPressedFire = new();
            private readonly FixedUpdateMechanics fixedUpdate = new();

            [Construct]
            public void Construct(Move move)
            {
                ShootEngine.Construct(BulletConfig, SpawnPointShoot);
                OnGetPressedFire += () =>
                {
                    ShootEngine.CreateBullet();
                };
                
                fixedUpdate.Construct(deltaTime => //TO DO убрать внутрь класса ShootEngine или вынести в отдельный класс 
                {
                    ShootEngine.Cooldown();
                }); 
            }

        }
    }
