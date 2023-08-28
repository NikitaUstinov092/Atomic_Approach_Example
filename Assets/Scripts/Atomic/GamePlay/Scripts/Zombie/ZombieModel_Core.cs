using System;
using Declarative;
using Lessons.Gameplay;
using Atomic.Components;
using UnityEngine;
using UnityEngine.Serialization;

namespace Atomic.GamePlay.Scripts.Zombie
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
            public AtomicVariable<Transform> Target = new();

            [SerializeField]
            public AtomicVariable<float> Speed = new();

            [SerializeField] public AtomicVariable<bool> IsChasing = new();

            private readonly FixedUpdateMechanics fixedUpdate = new();

            [Construct]
            public void Construct(HeroModel_Core.Life life, DistanceChecker distanceChecker)
            {
                var isDeath = life.isDeath;
                var closedToTarget = distanceChecker.ClosedTarget;

                fixedUpdate.Construct(deltaTime =>
                {
                    if (isDeath.Value || closedToTarget.Value)
                    {
                        IsChasing.Value = false;
                        return;
                    }
                    var targetPosition = Target.Value.position;
                    
                    moveTransform.position = Vector3.MoveTowards( moveTransform.position, targetPosition, Speed.Value * deltaTime);
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
            public AtomicVariable<Transform> Target = new();
            
            [SerializeField]
            public AtomicVariable<float> DistanceTarget = new();
            
            [SerializeField]
            public AtomicVariable<bool> ClosedTarget = new();
            
            private readonly FixedUpdateMechanics fixedUpdate = new();

            [Construct]
            public void Construct()
            {
                fixedUpdate.Construct(_ =>
                {
                    var distance = Vector3.Distance(moveTransform.position, Target.Value.position);
                    ClosedTarget.Value = distance < DistanceTarget.Value;
                   
                });
            }
        }


        [Serializable]
        public sealed class Attack
        {
            [SerializeField]
            public Entity AttackTarget;
            
            [SerializeField]
            public AtomicVariable<int> Damage = new();
            
            [SerializeField]
            public AtomicVariable<float> AttackDelay = new();
            
            [FormerlySerializedAs("CanAttack")] [SerializeField]
            public AtomicVariable<bool> StopAttack;
            
            private readonly FixedUpdateMechanics fixedUpdate = new();
            
            private IGetLifeComponent _playerLife;
            
            private float _timer;
            
            [Construct]
            public void Construct(DistanceChecker distanceChecker, HeroModel_Core.Life Life)
            {
                fixedUpdate.Construct(deltaTime=>
                {
                    if(Life.isDeath.Value)
                        return;
                    
                    if(StopAttack.Value)
                        return;
                    
                    if (_playerLife == null)  /// очень на счёт этого сомневаюсь =)))))
                    {
                        AttackTarget.TryGet(out IGetLifeComponent playerLife);  
                        _playerLife = playerLife;
                    }

                    if (_playerLife.GetLifeComponent().isDeath.Value)
                    {
                        StopAttack.Value = true;
                        return;
                    }
                    
                    if (!distanceChecker.ClosedTarget.Value)
                        return;

                    _timer += deltaTime;

                    if (!(_timer >= AttackDelay.Value)) 
                        return;
                    
                    if (AttackTarget.TryGet(out ITakeDamageComponent damage))
                        damage.TakeDamage(Damage.Value);
                    
                    _timer = 0f;
                });
            }
            
        }
    }
}