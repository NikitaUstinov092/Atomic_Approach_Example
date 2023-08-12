using System;
using Assets.Scripts.Common;
using Assets.Scripts.Custom;
using Lessons.Gameplay;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.GamePlay
{
    [Serializable]
    public sealed class HeroModel_Core
    {
        [SerializeField]
        public Transform Transform;

        public AtomicVariable<int> HitPoints = new();

        public AtomicEvent<int> OnTakeDamage = new();

        public AtomicEvent OnDeath = new();

        [Inject]
        public MoveEngine MoveEngine;
        public void Init()
        {
            MoveEngine.Construct(Transform);

            OnTakeDamage += damage =>
            {
                HitPoints.Value -= damage;
            };

            HitPoints.OnChanged += hp =>
            {
                if (hp <= 0)
                {
                    OnDeath?.Invoke();
                }
            };

            OnDeath += () => Debug.Log("Death");
        }
        
    }
}


