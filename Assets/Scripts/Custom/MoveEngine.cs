using Lessons.Gameplay;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Custom
{
    public sealed class MoveEngine: IFixedTickable
    {
        private Transform _transform;
        private IAtomicValue<float> _speed;

        private Vector3 _direction;
        private bool _moveRequired;

        public void Construct(Transform transform, IAtomicValue<float> speed)
        {
            _transform = transform;
            _speed = speed;
        }
        public void Move(Vector3 direction)
        {
            _direction = _transform.forward * direction.z + _transform.right * direction.x;
            _direction.Normalize(); 
            _moveRequired = true;
        }
        public void FixedTick()
        {
            if (_moveRequired)
            {
                _transform.position +=
                    _direction * (_speed.Value * Time.fixedDeltaTime);
                _moveRequired = false;
            }
        }
        
    }
}
