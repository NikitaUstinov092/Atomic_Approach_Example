using UnityEngine;

namespace Lessons.Gameplay.Atomic1
{
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