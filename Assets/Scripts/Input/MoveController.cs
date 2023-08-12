namespace Assets.Scripts.Input
{
    using GamePlay;
    using UnityEngine;
    public sealed class MoveController: MonoBehaviour
    {
        [SerializeField] 
        private HeroDocument _hero;

        [SerializeField] 
        private KeyCode _leftKey;

        [SerializeField]
        private KeyCode _rightKey;

        [SerializeField]
        private KeyCode _forwardKey;

        [SerializeField]
        private KeyCode _backKey;

        private void Update()
        {
            if (Input.GetKey(_leftKey))
            {
                _hero.Core.MoveEngine.Move(Vector3.left);
            }
            else if (Input.GetKey(_rightKey))
            {
                _hero.Core.MoveEngine.Move(Vector3.right);
            }
            else if (Input.GetKey(_forwardKey))
            {
                _hero.Core.MoveEngine.Move(Vector3.forward);
            }
            else if (Input.GetKey(_backKey))
            {
                _hero.Core.MoveEngine.Move(Vector3.back);
            }
        }
    }
}
