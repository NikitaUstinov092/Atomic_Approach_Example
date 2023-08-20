namespace ScriptableObject
{
    using UnityEngine;
    
    [CreateAssetMenu(fileName = "BulletConfig", menuName = "ScriptableObjects/BulletConfig")]
    public class BulletConfig: ScriptableObject
    {
        public Rigidbody Bullet;
        public float SpeedShoot;
        public Transform ShootPoint;
    }
}