using Assets.Scripts.Custom;
using Assets.Scripts.GamePlay;
using Assets.Scripts.Input;
using Zenject;

namespace Assets.Scripts.Common
{
    public sealed class SceneInstaller : MonoInstaller<SceneInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<HeroModel_Core>().AsSingle();
            Container.BindInterfacesAndSelfTo<MoveEngine>().AsSingle();
            Container.BindInterfacesAndSelfTo<RotationEngine>().AsSingle();
            
            Container.BindInterfacesAndSelfTo<HeroDocument>().FromComponentsInHierarchy().AsSingle();
            Container.BindInterfacesAndSelfTo<MoveInput>().FromComponentsInHierarchy().AsSingle();
            Container.BindInterfacesAndSelfTo<RotationInput>().FromComponentsInHierarchy().AsSingle();
        }
    }
}
