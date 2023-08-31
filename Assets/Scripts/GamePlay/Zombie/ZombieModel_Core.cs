using System;
using System.Atomic.Implementations;
using System.Declarative.Scripts.Attributes;
using GamePlay.Components.Interfaces;
using GamePlay.Hero;
using UnityEngine;
using UpdateMechanics;
using Random = UnityEngine.Random;

    namespace GamePlay.Zombie
    {
        [Serializable]
        public sealed class ZombieModel_Core
        {
            [Section]
            [SerializeField]
            public HeroModel_Core.Life life = new();

            [Section]
            [SerializeField] 
            public Chase ZombieChase = new();
        
            [Section]
            [SerializeField] 
            public DistanceChecker Distance = new();
        
            [Section]
            [SerializeField] 
            public Attack AttackHero = new();
        
            [Serializable]
            public sealed class Chase
            {
                [SerializeField]
                public Transform moveTransform;

                [SerializeField]
                public Entity.Entity Target = new();
            
                [SerializeField]
                public AtomicVariable<float> MinSpeed = new();
            
                [SerializeField]
                public AtomicVariable<float> MaxSpeed = new();
            
                private float _speed;

                [SerializeField] 
                public AtomicVariable<bool> IsChasing = new();

                private readonly FixedUpdateMechanics fixedUpdate = new();

                [Construct]
                public void Construct(HeroModel_Core.Life life, DistanceChecker distanceChecker)
                {
                    var isDeath = life.IsDead;
                    var closedToTarget = distanceChecker.ClosedTarget;
                    _speed = Random.Range(MinSpeed.Value, MaxSpeed.Value);

                    fixedUpdate.Construct(deltaTime =>
                    {
                        if (isDeath.Value || closedToTarget.Value || Target == null)
                        {
                            IsChasing.Value = false;
                            return;
                        }
                        var targetPosition = Target.transform.position;
                    
                        moveTransform.position = Vector3.MoveTowards( moveTransform.position, targetPosition, _speed * deltaTime);
                        moveTransform.LookAt(targetPosition);
                        IsChasing.Value = true;
                    });
                }
            }
        
            [Serializable]
            public sealed class DistanceChecker
            {
                [SerializeField]
                public Transform moveTransform;

                [SerializeField]
                public Entity.Entity Target = new();
            
                [SerializeField]
                public AtomicVariable<float> DistanceTarget = new();
            
                [SerializeField]
                public AtomicVariable<bool> ClosedTarget = new();
            
                private readonly FixedUpdateMechanics fixedUpdate = new();

                [Construct]
                public void Construct()
                {
                    var targetTransform = Target.transform;
                
                    fixedUpdate.Construct(_ =>
                    {
                        if (Target == null)
                            return;
                        var distance = Vector3.Distance(moveTransform.position, targetTransform.position);
                        ClosedTarget.Value = distance < DistanceTarget.Value;
                    });
                }
            }
        
            [Serializable]
            public sealed class Attack
            {
                [SerializeField]
                public Entity.Entity AttackTarget;
            
                [SerializeField]
                public AtomicVariable<int> Damage = new();
            
                [SerializeField]
                public AtomicVariable<float> AttackDelay = new();
            
                [SerializeField]
                public AtomicVariable<bool> StopAttack;
            
                private readonly LateUpdateMechanics _lateUpdate = new();
            
                private float _timer;
            
                [Construct]
                public void Construct(DistanceChecker distanceChecker, HeroModel_Core.Life Life)
                {
                    _lateUpdate.Construct(deltaTime=>
                    {
                        if(StopAttack.Value)
                            return;

                        if (AttackTarget == null || Life.IsDead.Value)
                        {
                            StopAttack.Value = true;
                            return;
                        }
                    
                        if (!distanceChecker.ClosedTarget.Value)
                            return;

                        _timer += deltaTime;

                        if (!(_timer >= AttackDelay.Value)) 
                            return;
                    
                        if (AttackTarget.TryGet(out ITakeDamagable damage))
                            damage.TakeDamage(Damage.Value);
                        _timer = 0f;
                    });
                }
            }
        }
    }
