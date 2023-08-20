using Sirenix.OdinInspector;

namespace Assets.Scripts.Actions
{
    public sealed class AtomicAction : Lessons.Gameplay.IAtomicAction
    {
        private readonly System.Action action;

        public AtomicAction(System.Action action)
        {
            this.action = action;
        }

        [Button]
        public void Invoke()
        {
            this.action?.Invoke();
        }
    }

    public sealed class AtomicAction<T> : Lessons.Gameplay.IAtomicAction<T>
    {
        private readonly System.Action<T> action;

        public AtomicAction(System.Action<T> action)
        {
            this.action = action;
        }

        [Button]
        public void Invoke(T args)
        {
            this.action?.Invoke(args);
        }
    }
}