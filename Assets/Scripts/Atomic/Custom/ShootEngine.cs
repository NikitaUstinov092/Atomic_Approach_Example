using ScriptableObject;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Custom
{
    public class ShootEngine: MonoBehaviour
    {
        private Transform _spawnPoint;
        private Rigidbody _bullet;
        private Vector3 _direction = Vector3.forward;
        
        private float _speed;
        private bool _shootRequired = true;
        private float _fireRate = 5f; // Частота выстрелов (раз в 5 секунд)
        private float _nextFireTime; // Время следующего выстрела
        
        public void Construct(BulletConfig bulletConfig)
        {
            _spawnPoint = bulletConfig.ShootPoint;
            _speed = bulletConfig.SpeedShoot;
            _bullet = bulletConfig.Bullet;
        }
        
        [Button]
        public void CreateBullet()
        {
            if (!_shootRequired)
            {
                return;
            }
            var bullet = Instantiate(_bullet,_spawnPoint.position, Quaternion.identity);
            bullet.AddForce(_direction * _speed);
            _shootRequired = false;
        }
        
        public void Cooldown()
        {
            if (!(Time.time >= _nextFireTime)) return;
            _shootRequired = true;
            
            _nextFireTime = Time.time + _fireRate; // Установка времени следующего выстрела
        }
    }
}