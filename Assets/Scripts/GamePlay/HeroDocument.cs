using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.GamePlay
{
    public sealed class HeroDocument: MonoBehaviour, IInitializable
    {
        [Inject]
        public HeroModel_Core Core;

        void IInitializable.Initialize()
        {
            Core.Transform = transform;
            Core.Speed.Value = 5f;
            Core.Init();
        }
    }
}
