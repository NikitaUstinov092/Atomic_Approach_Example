using Lessons.Gameplay;
using UnityEngine;

namespace Atomic.Components
{
    public interface IMoveComponent
    {
        void Move(Vector3 direction);
    }

    public sealed class MoveComponent : IMoveComponent
    {
        private readonly IAtomicAction<Vector3> onMove;

        public MoveComponent(IAtomicAction<Vector3> onMove)
        {
            this.onMove = onMove;
        }

        public void Move(Vector3 direction)
        {
            onMove.Invoke(direction);
        }
    }
}