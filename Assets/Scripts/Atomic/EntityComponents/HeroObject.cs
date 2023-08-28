using UnityEngine;

namespace Lessons.Gameplay.Atomic1
{
    public sealed class HeroObject : MonoBehaviour
    {
        public int HitPoints { get; private set; }
        public bool IsDeath { get; private set; }

        [SerializeField]
        private Transform moveTransform;

        [SerializeField]
        private float moveSpeed;

        private bool moveRequired;
        private Vector3 moveDirection;
        
        public void TakeDamage(int damage)
        {
            this.HitPoints -= damage;
            if (this.HitPoints <= 0)
            {
                this.IsDeath = true;
            }
        }

        public void Move(Vector3 direction)
        {
            if (this.IsDeath)
            {
                return;
            }

            this.moveDirection = direction;
            this.moveRequired = true;
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
    
    //Declarative Model
    // [Serializable]
    // public sealed class HeroObjectOptimized : IFixedUpdateListener
    // {
    //     public int HitPoints { get; private set; }
    //     public bool IsDeath { get; private set; }
    //
    //     [SerializeField]
    //     private Transform moveTransform;
    //
    //     [SerializeField]
    //     private float moveSpeed;
    //
    //     private bool moveRequired;
    //     private Vector3 moveDirection;
    //     
    //     public void TakeDamage(int damage)
    //     {
    //         this.HitPoints -= damage;
    //         if (this.HitPoints <= 0)
    //         {
    //             this.IsDeath = true;
    //         }
    //     }
    //
    //     public void Move(Vector3 direction)
    //     {
    //         if (this.IsDeath)
    //         {
    //             return;
    //         }
    //
    //         this.moveDirection = direction;
    //         this.moveRequired = true;
    //     }
    //     
    //     void IFixedUpdateListener.FixedUpdate(float deltaTime)
    //     {
    //         if (this.moveRequired)
    //         {
    //             this.moveTransform.position += this.moveDirection * (this.moveSpeed * Time.fixedDeltaTime);
    //             this.moveRequired = false;
    //         }
    //     }
    // }
}