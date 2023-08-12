using Assets.Scripts.Custom;
using Assets.Scripts.GamePlay;
using Zenject;

namespace Assets.Scripts.Common
{
    public class SceneInstaller : MonoInstaller<SceneInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<HeroModel_Core>().AsSingle();
            Container.BindInterfacesAndSelfTo<MoveEngine>().AsSingle();
            Container.BindInterfacesAndSelfTo<HeroDocument>().FromComponentsInHierarchy().AsSingle();
        }
    }
}
