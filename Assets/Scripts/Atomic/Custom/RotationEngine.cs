using System;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Custom
{
    [Serializable]
    public class RotationEngine
    {
        private Transform _transform;
        private Quaternion _targetRotation;
        private Camera _playerCamera;
        private const float _rotationSpeed = 3f;
        private Camera playerCamera;
       
        public void Construct(Transform transform, Camera playerCam)
        {
            _transform = transform;
            playerCamera = playerCam;
        }
        public void Rotate(Vector3 screenPos)
        {
            Debug.Log("++");
            Vector3 worldPos = _playerCamera.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, _playerCamera.transform.position.y));

            Vector3 direction = worldPos - _transform.position;
            direction.y = 0f;

            _targetRotation = Quaternion.LookRotation(direction);
        }
        public void FixedUpdate()
        {
            Debug.Log("FixedUpdate");
            _transform.rotation = Quaternion.Slerp(_transform.rotation, _targetRotation, _rotationSpeed * Time.deltaTime);
        }
    }
}