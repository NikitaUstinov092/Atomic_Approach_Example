using System;
using Assets.Scripts.Custom;
using Declarative;
using Lessons.Gameplay;
using ScriptableObject;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;


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
            
            [FormerlySerializedAs("onDeath")] [ShowInInspector]
            public AtomicEvent OnDeath = new();

            [FormerlySerializedAs("hitPoints")] [SerializeField]
            public AtomicVariable<int> HitPoints = new();

            [SerializeField]
            public AtomicVariable<bool> isDeath; //Нахер не нужна

            [Construct]
            public void Construct()
            {
                onTakeDamage += damage =>
                {
                    HitPoints.Value -= damage;
                };
                HitPoints.OnChanged += hitPoints =>
                {
                    if (hitPoints > 0) 
                        return;
                    isDeath.Value = true;
                    OnDeath?.Invoke();
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
            
            private AtomicVariable<Vector3> moveDirection = new();

            [SerializeField]
            public AtomicVariable<bool> moveRequired = new();
            
            [SerializeField]
            public AtomicVariable<float> speed = new();

            private readonly FixedUpdateMechanics fixedUpdate = new();

            [Construct]
            public void Construct(Life life)
            {
                var isDeath = life.isDeath;
                onMove += direction  =>
                {
                    moveDirection.Value = (moveTransform.forward * direction.z + moveTransform.right * direction.x).normalized;
                    moveRequired.Value = true;
                };

                fixedUpdate.Construct(deltaTime =>
                {
                    if (!moveRequired.Value || isDeath.Value) 
                        return;
                    
                    moveTransform.position += moveDirection.Value * (speed.Value * deltaTime);
                    moveRequired.Value = false;
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
                var isDeath = life.isDeath;

                RotationMotor.Construct(PlayerTransform, PlayerCamera, RotationSpeed.Value);
                
                fixedUpdate.Construct(_ =>
                {
                    if(isDeath.Value)
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
            public void Construct(Move move, Life life)
            {
                ShootEngine.Construct(BulletConfig, SpawnPointShoot);
                
                var isDead = life.isDeath;
                
                OnGetPressedFire += () =>
                {
                    if(isDead.Value)
                        return;
                    ShootEngine.CreateBullet();
                };
                
                fixedUpdate.Construct(deltaTime => //TO DO убрать внутрь класса ShootEngine или вынести в отдельный класс 
                {
                    if(isDead.Value)
                        return;
                    ShootEngine.Cooldown();
                }); 
            }

        }
    }
