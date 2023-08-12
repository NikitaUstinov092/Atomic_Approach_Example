using UnityEngine;

namespace Lessons.Gameplay.Atomic1
{
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
}