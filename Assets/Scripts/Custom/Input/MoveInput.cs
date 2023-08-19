using Assets.Scripts.Common;

using UnityEngine;
using Zenject;

namespace Assets.Scripts.Input
{
    public sealed class MoveInput : MonoBehaviour, IUpdateListener
    {
        /*[Inject]
        private HeroDocument _hero;*/

        [SerializeField] 
        private KeyCode _leftKey;

        [SerializeField] 
        private KeyCode _rightKey;

        [SerializeField] 
        private KeyCode _forwardKey;

        [SerializeField] 
        private KeyCode _backKey;

        void IUpdateListener.Update(float deltaTime)
        {
            /*if (UnityEngine.Input.GetKey(_leftKey))
            {
                _hero.Core.MoveEngine.Move(Vector3.left);
            }
            else if (UnityEngine.Input.GetKey(_rightKey))
            {
                _hero.Core.MoveEngine.Move(Vector3.right);
            }
            else if (UnityEngine.Input.GetKey(_forwardKey))
            {
                _hero.Core.MoveEngine.Move(Vector3.forward);
            }
            else if (UnityEngine.Input.GetKey(_backKey))
            {
                _hero.Core.MoveEngine.Move(Vector3.back);
            }*/
        }
    }
}
