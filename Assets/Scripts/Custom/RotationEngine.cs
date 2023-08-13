using UnityEngine;
using Zenject;

namespace Assets.Scripts.Custom
{
    public class RotationEngine : IFixedTickable
    {
        private Transform _transform;
        private bool _rotateRequired;
        private Quaternion _targetRotation;
        private Camera _playerCamera;
        private const float _rotationSpeed = 3f;
        public void Construct(Transform transform, Camera playerCamera)
        {
            _transform = transform;
            _playerCamera = playerCamera;
        }
        public void Rotate(Vector3 screenPos)
        {
            Vector3 worldPos = _playerCamera.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, _playerCamera.transform.position.y));

            Vector3 direction = worldPos - _transform.position;
            direction.y = 0f;

            _targetRotation = Quaternion.LookRotation(direction);
            _rotateRequired = true;
        }
        public void FixedTick()
        {
            if (_rotateRequired)
            {
                _transform.rotation = Quaternion.Slerp(_transform.rotation, _targetRotation, _rotationSpeed * Time.deltaTime);
                _rotateRequired = false;
            }
        }
    }
}