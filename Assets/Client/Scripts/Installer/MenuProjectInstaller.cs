using Zenject;

namespace Client
{
    public class MenuProjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<SceneLoader>().FromComponentInNewPrefabResource("SceneLoader").AsSingle().NonLazy();
        }
    }
}
