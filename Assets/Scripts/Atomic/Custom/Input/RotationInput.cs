using Lessons.Gameplay.Atomic1;
using UnityEngine;


namespace Assets.Scripts.Input
{
    public class RotationInput : MonoBehaviour
    {
        [SerializeField]
        private HeroModel _hero;
        void Update()
        {
            Vector3 screenPos = UnityEngine.Input.mousePosition;
            _hero.Core.rotate.onRotate.Invoke(screenPos);
        }
    }
}