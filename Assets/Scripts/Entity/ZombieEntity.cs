using GamePlay.Components;
using GamePlay.Zombie;
using UnityEngine;

namespace Entity
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
