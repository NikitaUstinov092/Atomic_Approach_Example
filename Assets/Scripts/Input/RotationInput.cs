using Assets.Scripts.Common;
using Assets.Scripts.GamePlay;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Input
{
    public class RotationInput : MonoBehaviour, IUpdateListener
    {
        [Inject]
        private HeroDocument _hero;
        void IUpdateListener.Update(float deltaTime)
        {
            Vector3 screenPos = UnityEngine.Input.mousePosition;
            _hero.Core.RotateEngine.Rotate(screenPos);
        }
    }
}
