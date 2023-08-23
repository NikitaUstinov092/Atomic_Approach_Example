namespace ScriptableObject
{
    using UnityEngine;
    
    [CreateAssetMenu(fileName = "BulletConfig", menuName = "ScriptableObjects/BulletConfig")]
    public class BulletConfig: ScriptableObject
    {
        public Rigidbody Bullet;
        public float SpeedShoot;
        public float CoolDown;
        public Transform ShootPoint;
    }
}