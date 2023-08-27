using Atomic.Components;
using UnityEngine;

public class CollideHitEntity : MonoBehaviour
{
   [SerializeField] 
   private int _damage;
   private void OnCollisionEnter(Collision other)
   {
      if (!other.gameObject.TryGetComponent(out Entity entity)) 
         return;
      if(entity.TryGet(out ITakeDamageComponent damage))
         damage.TakeDamage(_damage);
      Destroy(gameObject);
   }
}
