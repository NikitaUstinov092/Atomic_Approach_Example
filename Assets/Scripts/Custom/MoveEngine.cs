using System;
using Lessons.Gameplay;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Custom
{
    public sealed class MoveEngine: IFixedTickable
    {
        private Transform _transform;
        private float _speed = 5f;

        private Vector3 _direction;
        private bool _moveRequired = false;

        public void Construct(Transform transform)
        {
            _transform = transform;
        }
        public void Move(Vector3 direction)
        {
            _direction = direction;
            _moveRequired = true;
        }
        public void FixedTick()
        {
            if (_moveRequired)
            {
                _transform.position += _direction * (_speed * Time.fixedDeltaTime);
                _moveRequired = false;
            }
        }
        
    }
}
