using Lessons.Gameplay.Atomic1;
using UnityEngine;


namespace Assets.Scripts.Input
{
    public sealed class MoveInput : MonoBehaviour
    {
        [SerializeField]
        private HeroModel _hero;

        [SerializeField] 
        private KeyCode _leftKey;

        [SerializeField] 
        private KeyCode _rightKey;

        [SerializeField] 
        private KeyCode _forwardKey;

        [SerializeField] 
        private KeyCode _backKey;

        private Vector3 _direction;
        
        private void Update()
        {
            if (UnityEngine.Input.GetKey(_leftKey))
            {
                _direction = Vector3.left;
                ApplyMove();
            }
            else if (UnityEngine.Input.GetKey(_rightKey))
            {
                _direction = Vector3.right;
                ApplyMove();
            }
            else if (UnityEngine.Input.GetKey(_forwardKey))
            {
                _direction = Vector3.forward;
                ApplyMove();
            }
            else if (UnityEngine.Input.GetKey(_backKey))
            {
                _direction = Vector3.back;
                ApplyMove();
            }
        }

        private void ApplyMove()
        {
            _hero.Core.move.onMove.Invoke(_direction);
        }
    }
}
