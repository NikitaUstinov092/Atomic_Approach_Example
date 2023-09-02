using GamePlay.Components;
using GamePlay.Hero;
using UnityEngine;

namespace Entity
{
    public class HeroEntity : Entity
    {
        [SerializeField]
        private HeroModel model;
        private void Awake()
        {
            Add(new MoveComponent(model.Core.move.onMove));
            Add(new TakeDamageComponent(model.Core.life.OnTakeDamage));
            Add(new RotateComponent(model.Core.rotate.RotateDirection));
            Add(new LifeComponent(model.Core.life));
        }
    }
}
