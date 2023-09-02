using System;
using GamePlay.Components;
using GamePlay.Custom;
using GamePlay.Hero;
using UnityEngine;
using Zenject;

public class KillsCounter<T>: IStartListener, IDisableListener, IKillCounterPM where T: Entity.Entity
{
    public event Action<int> OnValueChanged;
    
    [Inject]
    private IEnemyFactory<T> _enemyFactory;
    
    private T _enemy;
    private int _deathCount;
    void IStartListener.StartGame()
    {
        _enemyFactory.OnEnemyCreated += CheckLifeComponent;
    }
    void IDisableListener.Disable()
    {
        _enemyFactory.OnEnemyCreated -= CheckLifeComponent;
    }

    private void CheckLifeComponent(T enemy)
    {
        if (enemy.TryGet(out IGetLifeComponent lifeComp))
        {
            AddListenerDeathCount(lifeComp.GetLifeComponent());
            return;
        }
        Debug.LogError($"Компонент Life не найден на сущности {enemy.name}");
    }
    private void AddListenerDeathCount(HeroModel_Core.Life lifeComp)
    {
        lifeComp.OnDeath.Subscribe(CountDeath);
    }
    
    private void CountDeath()
    {
       OnValueChanged?.Invoke(++_deathCount);
    }
}

public interface IKillCounterPM
{
    event Action<int> OnValueChanged;
}
