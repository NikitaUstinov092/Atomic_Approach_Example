using System;
using Declarative;


    public sealed class LateUpdateMechanics : ILateUpdateListener
    {
        private Action<float> action;

        public void Construct(Action<float> action)
        {
            this.action = action;
        }

        void ILateUpdateListener.LateUpdate(float deltaTime)
        {
            this.action.Invoke(deltaTime);
        }

        public void SetAction(Action<object> action1)
        {
            throw new NotImplementedException();
        }
    }
