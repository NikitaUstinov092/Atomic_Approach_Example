using System;
using System.Atomic.Implementations;
using System.Declarative.Scripts.Attributes;
using GamePlay.Custom;
using GamePlay.Custom.ScriptableObjects;
using Sirenix.OdinInspector;
using UnityEngine;
using UpdateMechanics;
using Object = UnityEngine.Object;


    namespace GamePlay.Hero
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
        
            [Section]
            [SerializeField]
            public EntityContainer EntityStorage = new();
        
            [Serializable]
            public sealed class Life
            {
                [ShowInInspector]
                public AtomicEvent<int> OnTakeDamage = new();
            
                [ShowInInspector]
                public AtomicEvent OnDeath = new();

                [SerializeField]
                public AtomicVariable<int> HitPoints = new();

                [SerializeField]
                public AtomicVariable<bool> IsDead;
            
                [Construct]
                public void Construct()
                {
                    OnTakeDamage.Subscribe(damage =>
                    {
                        if (IsDead.Value)
                            return;
                        HitPoints.Value -= damage;
                    });
                
                    HitPoints.onChanged += hitPoints =>
                    {
                        if (hitPoints > 0) 
                            return;
                        IsDead.Value = true;
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
                    var isDeath = life.IsDead;
                    onMove.Subscribe(direction =>
                    {
                        moveDirection.Value = (moveTransform.forward * direction.z + moveTransform.right * direction.x)
                            .normalized;
                        moveRequired.Value = true;
                    });

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
                    var isDeath = life.IsDead;

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
                
                    var isDead = life.IsDead;
                
                    OnGetPressedFire.Subscribe(() =>
                    {
                        if(isDead.Value)
                            return;
                    
                        ShootEngine.CreateBullet();
                    });
                
                    fixedUpdate.Construct(deltaTime => //TO DO убрать внутрь класса ShootEngine или вынести в отдельный класс 
                    {
                        if(isDead.Value)
                            return;
                    
                        ShootEngine.Cooldown();
                    }); 
                }
            }
        
            public sealed class Ammo
            {
            
            
            }
        

            [Serializable]
            public sealed class EntityContainer
            {
                public Entity.Entity Entity;

                [Construct]
                public void Construct(Life life)
                {
                    life.OnDeath.Subscribe(() =>
                    {
                        Object.Destroy(Entity);
                    });
                }
            
            }
        
        }
    }
