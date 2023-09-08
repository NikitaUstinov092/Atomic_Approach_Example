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
            Add(new MoveComponent(model.Core.MoveComp.OnMove));
            Add(new TakeDamageComponent(model.Core.LifeComp.OnTakeDamage));
            Add(new RotateComponent(model.Core.RotateComp.RotationDirection));
            Add(new LifeComponent(model.Core.LifeComp));
            Add(new ShootComponent(model.Core.ShootComp.OnGetPressedFire));
        }
    }
}
