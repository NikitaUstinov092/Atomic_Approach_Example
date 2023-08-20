using System;
using Assets.Scripts.Custom;
using Declarative;
using Lessons.Gameplay;
using Sirenix.OdinInspector;
using UnityEngine;

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
                this.onMove += direction =>
                {
                    if (isDeath.Value)
                    {
                        return;
                    }

                    this.moveDirection.Value = direction;
                    this.moveRequired.Value = true;
                };

                this.fixedUpdate.Construct(deltaTime =>
                {
                    if (this.moveRequired.Value)
                    {
                        this.moveTransform.localPosition += this.moveDirection.Value * (this.speed.Value * deltaTime);
                        this.moveRequired.Value = false;
                    }
                });
            }
        }
        [Serializable]
        public sealed class Rotate
        {
            [SerializeField]
            public Camera cam;
            
            [Section]
            public RotationEngine rotationMotor = new();
            
            [SerializeField]
            public Transform RTransform;
            
            [ShowInInspector]
            public AtomicEvent<Vector3> onRotate = new();
            
            private readonly FixedUpdateMechanics fixedUpdate = new();
            
            [SerializeField]
            public AtomicVariable<Vector3> rotateDirection = new();

            [SerializeField]
            public AtomicVariable<float> speed = new();
            
            [Construct]
            public void Construct(Life life)
            {
                var isDeath = life.isDeath;
                
                rotationMotor.Construct(RTransform, cam);
                
                
                /*
                onRotate += rotateDir =>
                {
                    if (isDeath.Value)
                    {
                        return;
                    }
                };*/
                onRotate += rotateDir => rotationMotor.Rotate(rotateDir);
                
                this.fixedUpdate.Construct(deltaTime =>
                {
                    rotationMotor.FixedUpdate();
                });
            }
        }
        
    }
}