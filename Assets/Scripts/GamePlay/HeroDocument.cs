using UnityEngine;
using Zenject;

namespace Assets.Scripts.GamePlay
{
    public sealed class HeroDocument: MonoBehaviour, IInitializable
    {
        [Inject]
        [SerializeField]
        public HeroModel_Core Core;

        void IInitializable.Initialize()
        {
            Core.Transform = transform;
            Core.Init();
        }
    }
}
