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
        private BulletConfig _config;
        private GameObject _parent;
        
        private float _shootSpeed;
        private bool _shootRequired = true;
        private float _fireRate; 
        private float _nextFireTime;
        
        public void Construct(BulletConfig bulletConfig, Transform spawnPoint)
        {
            _shootSpeed = bulletConfig.SpeedShoot;
            _bullet = bulletConfig.Bullet;
            _fireRate = bulletConfig.CoolDown;
            _spawnPoint = spawnPoint;
            
            CreateParent();
        }
        private void CreateParent()
        {
            _parent = new GameObject("Bullets");
        }
        
        [Button]
        public void CreateBullet()
        {
            if (!_shootRequired)
                return;
            
            var bullet = Instantiate(_bullet,_spawnPoint.position, _spawnPoint.rotation);
            bullet.transform.parent = _parent.transform;
            Shoot(bullet);
        }
        
        [Button]
        private void Shoot(Rigidbody bullet)
        {
            var rotation = _spawnPoint.rotation;
            var localShootDirection = rotation * Vector3.forward;
            bullet.velocity = localShootDirection * _shootSpeed * Time.deltaTime;
            _shootRequired = false;
        }
        
        public void Cooldown()
        {
            if (!(Time.time >= _nextFireTime)) return;
            _shootRequired = true;
            
            _nextFireTime = Time.time + _fireRate; 
        }
    }
}