using UnityEngine;

namespace Lessons.Gameplay.Atomic1
{
    public sealed class HitPointsComponent
    {
        public int HitPoints { get; set; }
    }

    public sealed class DeathComponent
    {
        public bool IsDeath { get; set; }
    }
    
    [RequireComponent(typeof(HitPointsComponent))]
    [RequireComponent(typeof(DeathComponent))]
    public sealed class TakeDamageComponent : MonoBehaviour
    {
        private HitPointsComponent hitPointsComponent;
        private DeathComponent deathComponent;
    
        public void TakeDamage(int damage)
        {
            this.hitPointsComponent.HitPoints -= damage;
            if (this.hitPointsComponent.HitPoints <= 0)
            {
                this.deathComponent.IsDeath = true;
            }
        }

        private void Awake()
        {
            this.hitPointsComponent = this.GetComponent<HitPointsComponent>();
            this.deathComponent = this.GetComponent<DeathComponent>();
        }
    }

    [RequireComponent(typeof(DeathComponent))]
    public sealed class MoveComponent : MonoBehaviour
    {
        [SerializeField]
        private Transform moveTransform;

        [SerializeField]
        private float moveSpeed;

        private bool moveRequired;
        private Vector3 moveDirection;

        private DeathComponent deathComponent;

        public void Move(Vector3 direction)
        {
            if (this.deathComponent.IsDeath)
            {
                return;
            }

            this.moveDirection = direction;
            this.moveRequired = true;
        }

        private void Awake()
        {
            this.deathComponent = this.GetComponent<DeathComponent>();
        }

        private void FixedUpdate()
        {
            if (this.moveRequired)
            {
                this.moveTransform.position += this.moveDirection * (this.moveSpeed * Time.fixedDeltaTime);
                this.moveRequired = false;
            }
        }
    }
}