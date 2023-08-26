﻿using System;
using System.Diagnostics.Contracts;
using Declarative;
using Lessons.Gameplay;
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
        
        [Serializable]
        public sealed class Chase
        {
            [SerializeField]
            public Transform moveTransform;

            [SerializeField]
            public AtomicVariable<Transform> Target = new();

            [SerializeField]
            public AtomicVariable<float> Speed = new();

            public AtomicVariable<Vector3> moveDirection = new();

            private readonly FixedUpdateMechanics fixedUpdate = new();

            [Construct]
            public void Construct(HeroModel_Core.Life life, DistanceChecker distanceChecker)
            {
                var isDeath = life.isDeath;
                var closedToTarget = distanceChecker.ClosedTarget;

                fixedUpdate.Construct(deltaTime =>
                {
                    if (isDeath.Value || closedToTarget.Value)
                        return;

                    var targetPosition = Target.Value.position;
                    
                    moveTransform.position = Vector3.MoveTowards( moveTransform.position, targetPosition, Speed.Value * deltaTime);
                    moveTransform.LookAt(targetPosition);
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
            
            private readonly LateUpdateMechanics lateUpdate = new();

            [Construct]
            public void Construct()
            {
                lateUpdate.Construct(deltaTime =>
                {
                    var distance = Vector3.Distance(moveTransform.position, Target.Value.position);
                    ClosedTarget.Value = distance < DistanceTarget.Value;
                });
            }
        }
    }
}