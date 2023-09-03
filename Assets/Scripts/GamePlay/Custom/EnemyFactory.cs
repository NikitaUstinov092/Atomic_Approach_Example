using System;
using System.Collections;
using GamePlay.Components.Interfaces;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GamePlay.Custom
{
    public class EnemyFactory : MonoBehaviour, IStartListener, IEnemyFactory<Entity.Entity>
    {
        public event Action<Entity.Entity> OnEnemyCreated;
        
        [SerializeField] 
        private Entity.Entity _enemy;
    
        [SerializeField]  
        private Entity.Entity _targetEntity;

        [SerializeField] 
        private Transform[] _spawnPoints;

        [SerializeField] 
        private float _delaySpawn = 2;

        private GameObject _parent;

        void IStartListener.StartGame()
        {
            _parent = new GameObject("Enemies");
            TargetState();
            StartCoroutine(Spawn());
        }
        private IEnumerator Spawn()
        {
            while (TargetState())
            {
                var spawnEntity =  Instantiate(_enemy, _spawnPoints[Random.Range(0, _spawnPoints.Length)]
                    .position,Quaternion.identity);
                
                spawnEntity.transform.parent = _parent.transform;
                
                AddTarget(spawnEntity);
                
                OnEnemyCreated?.Invoke(spawnEntity);
                
                yield return new WaitForSeconds(_delaySpawn);
            }
        }
        
        private void AddTarget(Entity.Entity enemy)
        {
            if(enemy.TryGet(out ISetEntityTargetComponent setEntityComp))
                setEntityComp.SetEntityTarget(_targetEntity);
            else
                throw new NullReferenceException();
        }

        private bool TargetState()
        {
           return _targetEntity != null; 
        }
    }

    public interface IEnemyFactory<T>
    {
       event Action<T> OnEnemyCreated;
    }
}
