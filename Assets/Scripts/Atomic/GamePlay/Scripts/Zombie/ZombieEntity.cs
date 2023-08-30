using Atomic.Components;
using UnityEngine;

namespace Atomic.GamePlay.Scripts.Zombie
{
    public class ZombieEntity: Entity
    {
        [SerializeField]
        private ZombieModel model;
        private void Awake()
        {
            Add(new TakeDamageComponent(model.Core.life.OnTakeDamage));
        }
    }
}