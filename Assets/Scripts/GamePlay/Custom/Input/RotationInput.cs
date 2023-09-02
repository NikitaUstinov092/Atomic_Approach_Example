using GamePlay.Hero;
using UnityEngine;

namespace GamePlay.Custom.Input
{
    public class RotationInput : MonoBehaviour, IUpdateListener
    {
        [SerializeField]
        private HeroModel _hero;

        void IUpdateListener.Update()
        {
            var screenPos = UnityEngine.Input.mousePosition;
            _hero.Core.rotate.RotateDirection.Value = screenPos;
        }
    }
}

