using UnityEngine;

namespace Lessons.Gameplay.Atomic2
{
    public sealed class MoveComponent : IMoveComponent
    {
        private readonly IAtomicAction<Vector3> onMove;

        public MoveComponent(IAtomicAction<Vector3> onMove)
        {
            this.onMove = onMove;
        }

        public void Move(Vector3 direction)
        {
            this.onMove.Invoke(direction);
        }
    }
}