using UnityEngine;

namespace Lessons.Gameplay.Atomic2
{
    public interface IMoveComponent
    {
        void Move(Vector3 direction);
    }
}