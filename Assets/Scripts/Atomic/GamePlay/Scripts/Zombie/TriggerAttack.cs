using Atomic.Components;
using Sirenix.OdinInspector;
using UnityEngine;

public class TriggerAttack : MonoBehaviour
{
    [ReadOnly]
    [SerializeField]
    private Entity _targetEntity;

    [SerializeField]
    private PeakAttack _pickAttack;

    [SerializeField] private int _damage;
    
    private void OnTriggerStay(Collider other)
    {
        if (_targetEntity)
        {
            if(!_pickAttack.GetState())
                return;
            
            if(_targetEntity.TryGet(out ITakeDamageComponent damage))
                damage.TakeDamage(_damage);
            return;
        }
            
        if (other.TryGetComponent(out Entity entity))
                _targetEntity = entity;
    }

    private void OnTriggerExit(Collider other)
    {
        _targetEntity = null;
    }
}
