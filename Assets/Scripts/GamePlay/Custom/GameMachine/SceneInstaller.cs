using Entity;
using GamePlay.Custom;
using GamePlay.Custom.Input;
using Zenject;

public class SceneInstaller : MonoInstaller<SceneInstaller>
{
    public override void InstallBindings()
    {
        Container.Bind<HeroEntity>().FromComponentsInHierarchy().AsSingle();
        Container.BindInterfacesAndSelfTo<EnemyFactory>().FromComponentsInHierarchy().AsSingle();
        Container.BindInterfacesTo<KillsCounter<Entity.Entity>>().AsSingle();
        Container.BindInterfacesTo<MoveInput>().FromComponentsInHierarchy().AsSingle();
        Container.BindInterfacesTo<RotationInput>().FromComponentsInHierarchy().AsSingle();
        Container.BindInterfacesTo<ShootInput>().FromComponentsInHierarchy().AsSingle();
    }
    
}