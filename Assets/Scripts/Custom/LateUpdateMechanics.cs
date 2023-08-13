using System;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Custom
{
    public sealed class LateUpdateMechanics : ILateTickable
    {
        private Action<float> _update;

        public void OnUpdate(Action<float> update) 
        {
            _update = update;
        }
        public void LateTick()
        {
            _update?.Invoke(Time.deltaTime);
        }
    }
}
