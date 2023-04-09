using UnityEngine;
using Zenject;

namespace Client
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private PlayerBehaviour _playerBehaviour;
        public override void InstallBindings()
        {
            Container.Bind<PlayerBehaviour>().FromInstance(_playerBehaviour).AsSingle().NonLazy();
        }
    }
}
