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
            Add(new TakeDamageComponent(model.Core.Life.OnTakeDamage));
            Add(new SetTargetEntityComponent(model.Core.TargetDistance));
            Add(new LifeComponent(model.Core.Life));
        }
    }
}
