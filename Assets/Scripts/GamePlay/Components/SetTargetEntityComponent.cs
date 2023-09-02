using GamePlay.Components.Interfaces;
using GamePlay.Zombie;

public class SetTargetEntityComponent: ISetEntityTargetComponent
{
    private readonly ZombieModel_Core.TargetDistanceChecker _targetDistanceChecker;
    
    public SetTargetEntityComponent(ZombieModel_Core.TargetDistanceChecker targetDistanceChecker)
    {
        _targetDistanceChecker = targetDistanceChecker;
    }
    
    void ISetEntityTargetComponent.SetEntityTarget(Entity.Entity entity)
    {
        _targetDistanceChecker.Target.Value = entity;
    }
}
