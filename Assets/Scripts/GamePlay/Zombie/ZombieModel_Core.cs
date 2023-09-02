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
            public HeroModel_Core.Life Life = new();

            [Section]
            [SerializeField] 
            public Chase ZombieChase = new();
            
            [Section]
            [SerializeField] 
            public TargetDistanceChecker TargetDistance = new();
        
            [Section]
            [SerializeField] 
            public Attack AttackHero = new();
        
            [Serializable]
            public sealed class Chase
            {
                [SerializeField]
                public Transform MoveTransform;
                public AtomicVariable<float> MinSpeed = new();
                public AtomicVariable<float> MaxSpeed = new();
                public AtomicVariable<bool> IsChasing = new();
            
                private float _speed;
                private readonly FixedUpdateMechanics _fixedUpdate = new();

                [Construct]
                public void Construct(HeroModel_Core.Life life, TargetDistanceChecker targetDistanceChecker)
                {
                    var isDeath = life.IsDead;
                    var closedToTarget = targetDistanceChecker.ClosedTarget;
                    
                    _speed = Random.Range(MinSpeed.Value, MaxSpeed.Value);

                    _fixedUpdate.Construct(deltaTime =>
                    {
                        if (isDeath.Value || closedToTarget.Value || targetDistanceChecker.Target.Value == null)
                        {
                            IsChasing.Value = false;
                            return;
                        }
                        var targetPosition = targetDistanceChecker.Target.Value.transform.position;
                    
                        MoveTransform.position = Vector3.MoveTowards( MoveTransform.position, targetPosition, _speed * deltaTime);
                        MoveTransform.LookAt(targetPosition);
                        IsChasing.Value = true;
                    });
                }
            }
        
            [Serializable]
            public sealed class TargetDistanceChecker
            {
                [SerializeField]
                public Transform MoveTransform;
                
                public AtomicVariable<Entity.Entity> Target = new();
                
                public AtomicVariable<float> DistanceTarget = new();
                
                public AtomicVariable<bool> ClosedTarget = new();
            
                private readonly FixedUpdateMechanics _fixedUpdate = new();

                [Construct]
                public void Construct()
                {
                    _fixedUpdate.Construct(_ =>
                    {
                        if(Target.Value == null)
                            return;
                        var distance = Vector3.Distance(MoveTransform.position, Target.Value.transform.position);
                        ClosedTarget.Value = distance < DistanceTarget.Value;
                    });
                }
            }
        
            [Serializable]
            public sealed class Attack
            {
                public AtomicVariable<int> Damage = new();
                public AtomicVariable<float> AttackDelay = new();
                public AtomicVariable<bool> StopAttack = new();
                
                private readonly LateUpdateMechanics _lateUpdate = new();
                private float _timer;
            
                [Construct]
                public void Construct(TargetDistanceChecker targetDistanceChecker, HeroModel_Core.Life life)
                {
                    _lateUpdate.Construct(deltaTime=>
                    {
                        StopAttack.Value = targetDistanceChecker.Target.Value == null && life.IsDead.Value;
                       
                        if(StopAttack.Value)
                            return;
                    
                        if (!targetDistanceChecker.ClosedTarget.Value)
                            return;

                        _timer += deltaTime;
                        
                        if (!(_timer >= AttackDelay.Value)) 
                            return;
                        
                        if (targetDistanceChecker.Target.Value != null && 
                            targetDistanceChecker.Target.Value.TryGet(out ITakeDamagable damage))
                            damage.TakeDamage(Damage.Value);
                        
                        _timer = 0f;
                    });
                }
            }
        }
    }
