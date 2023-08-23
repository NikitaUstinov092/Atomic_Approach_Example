using UnityEngine;

public class BulletCleaner : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
       if(other.CompareTag("Bullet"))
           Destroy(other.gameObject);
    }
}
