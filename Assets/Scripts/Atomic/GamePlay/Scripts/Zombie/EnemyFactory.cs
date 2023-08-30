using System.Collections;
using Atomic.GamePlay.Scripts.Zombie;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyFactory : MonoBehaviour
{
    [SerializeField] 
    private ZombieModel _enemy;
    
    [SerializeField]  
    private Entity _heroEntity;

    [SerializeField] 
    private Transform[] _spawnPoints;

    [SerializeField] 
    private float _delay = 2;


    private IEnumerator Start()
    {
        while (_heroEntity!=null)
        {
            var enemy =  Instantiate(_enemy, _spawnPoints[Random.Range(0, _spawnPoints.Length)].position, 
                        quaternion.identity);
            AddDependencies(enemy);
            enemy.Initialize();
            yield return new WaitForSeconds(_delay);
        }
    }


    private void AddDependencies(ZombieModel zombie)
    {
        var core = zombie.Core;
        core.ZombieChase.Target = _heroEntity;
        core.AttackHero.AttackTarget = _heroEntity;
    }
}
