using Lessons.Gameplay.Atomic1;
using UnityEngine;
using MoveComponent = Atomic.Components.MoveComponent;

public class HeroEntity : Entity
{
    [SerializeField]
    private HeroModel model;
    private void Awake()
    {
        this.Add(new MoveComponent(model.Core.move.onMove));
    }
}
