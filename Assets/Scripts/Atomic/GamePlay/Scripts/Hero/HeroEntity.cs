using Lessons.Gameplay.Atomic1;
using UnityEngine;
using MoveComponent = Atomic.Components.MoveComponent;
using TakeDamageComponent = Atomic.Components.TakeDamageComponent;


public class HeroEntity : Entity
{
    [SerializeField]
    private HeroModel model;
    private void Awake()
    {
        Add(new MoveComponent(model.Core.move.onMove));
        Add(new TakeDamageComponent(model.Core.life.onTakeDamage));
        Add(new LifeComponent(model.Core.life));
    }
}
