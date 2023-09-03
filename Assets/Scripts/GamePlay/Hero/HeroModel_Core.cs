using System;
using System.Atomic.Implementations;
using System.Declarative.Scripts.Attributes;
using GamePlay.Custom;
using GamePlay.Custom.ScriptableObjects;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
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
            public Ammo ammo = new();
        
            [Section]
            [SerializeField]
            public EntityContainer EntityStorage = new();
        
            [Serializable]
            public class Life
            {
                public AtomicEvent<int> OnTakeDamage = new();
                public AtomicEvent OnDeath = new();
                public AtomicVariable<int> HitPoints = new();
                public AtomicVariable<bool> IsDead= new();
            
                [Construct]
                public void Construct()
                {
                    OnTakeDamage.Subscribe(damage =>
                    {
                        if (IsDead.Value)
                            return;
                        
                        HitPoints.Value -= damage;

                        if (HitPoints.Value != 0)
                            return;
                       
                        IsDead.Value = true;
                        OnDeath?.Invoke();
                    });
                }
            }

            [Serializable]
            public class Move
            {
                [ShowInInspector]
                public AtomicEvent<Vector3> OnMove = new();
                
                public AtomicVariable<float> Speed = new();
                public AtomicVariable<bool> MoveRequired = new ();

                private readonly FixedUpdateMechanics _fixedUpdate = new();
                
                [SerializeField]
                private Transform _moveTransform;
                
                private Vector3 _moveDirection;
                
                [Construct]
                public void Construct(Life life)
                {
                    var isDeath = life.IsDead;
                    
                    OnMove.Subscribe(direction =>
                    {
                        if(isDeath.Value)
                            return;
                        _moveDirection = (_moveTransform.forward * direction.z + _moveTransform.right * direction.x)
                            .normalized;
                        MoveRequired.Value = true;
                    });

                    _fixedUpdate.Construct(deltaTime =>
                    {
                        if (!MoveRequired.Value || isDeath.Value) 
                            return;
                    
                        _moveTransform.position += _moveDirection * (Speed.Value * deltaTime);
                        MoveRequired.Value = false;
                    });
                }
            }
            [Serializable]
            public sealed class Rotate
            {
                public AtomicVariable<Vector3> RotateDirection;
            
                public AtomicVariable<float> RotationSpeed;
           
                [SerializeField]
                private Camera _playerCamera;
                
                [SerializeField]
                private Transform _playerTransform;
                
                private readonly FixedUpdateMechanics _fixedUpdate = new();
                private RotationEngine _rotationMotor = new();

                [Construct]
                public void Construct(Life life)
                {
                    var isDeath = life.IsDead;

                    _rotationMotor.Construct(_playerTransform, _playerCamera, RotationSpeed.Value);
                
                    _fixedUpdate.Construct(_ =>
                    {
                        if(isDeath.Value)
                            return;
                    
                        var cursorScreenPos = RotateDirection.Value;
                        _rotationMotor.UpdateRotation(cursorScreenPos);
                    });
                }
            }

            [Serializable]
            public sealed class Shoot
            {
                public AtomicEvent OnGetPressedFire = new();
                public AtomicEvent OnBulletCreated = new();
                
                public AtomicVariable<BulletConfig> BulletConfig = new();
                public AtomicVariable<float> CoolDown = new();
                
                [SerializeField]
                private ShootEngine _shootEngine;
                
                [SerializeField]
                private Transform _spawnPointShoot;
                
                private readonly FixedUpdateMechanics _fixedUpdate = new();
                
                private bool _canShoot = true;
                private bool _coolDown;
                private float _timer;
                
                [Construct]
                public void Construct(Ammo ammo, Life life)
                {
                    _shootEngine.Construct(BulletConfig.Value, _spawnPointShoot);
                
                    var isDead = life.IsDead;
                    var ammoCount = ammo.AmmoCount;
                    
                    isDead.Subscribe((data) => _canShoot = !data);
                    ammoCount.Subscribe((count) => _canShoot = (count > 0));

                    if (ammoCount.Value <= 0)
                        _canShoot = false;
                
                    OnGetPressedFire.Subscribe(() =>
                    {
                        if(!_canShoot || _coolDown)
                            return;
                    
                        _coolDown = true;
                        _shootEngine.CreateBullet();
                        OnBulletCreated?.Invoke();
                    });
                
                    _fixedUpdate.Construct(deltaTime => 
                    {
                        if(!_canShoot || !_coolDown)
                            return;
                        
                        _timer += deltaTime;

                        if (_timer <= CoolDown.Value || !_canShoot) 
                            return;
                        
                        _timer = 0;
                        _coolDown = false;
                    }); 
                }
            }
        
            [Serializable]
            public sealed class Ammo
            {
                public AtomicVariable<int> AmmoCount = new();
                public AtomicVariable<int> MaxAmmo = new ();
                public AtomicVariable<float> ReloadTime = new ();
                
                private readonly FixedUpdateMechanics _fixedUpdate = new();
                
                private bool _reloadRequired;
                private bool _canReload = true;
                private float _timer;
                
                [Construct]
                public void Construct(Shoot shootComp, Life lifeComp)
                {
                    var isDead = lifeComp.IsDead;
                    var shoot = shootComp.OnBulletCreated;
                    
                    isDead.Subscribe((data) => _canReload = !data);
                    
                    AmmoCount.Subscribe(count => _reloadRequired = count < MaxAmmo.Value);

                    if (AmmoCount.Value < MaxAmmo.Value)
                        _reloadRequired = true;
                    
                    shoot.Subscribe(() =>
                    {
                        if(AmmoCount.Value <= 0 || !_canReload)
                            return;
                        AmmoCount.Value--;
                    });
                    
                    _fixedUpdate.Construct(deltaTime => 
                    {
                        if(!_reloadRequired || !_canReload)
                            return;
                    
                        _timer += deltaTime;

                        if (_timer <= ReloadTime.Value || !_canReload) 
                            return;
                        
                        AmmoCount.Value++;
                        _timer = 0;
                    }); 
                }
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
