using UnityEngine;
using Zenject;

namespace Client
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private PlayerViewer _playerViewer;
        [SerializeField] private PlayerBehaviour _playerBehaviour;
        [SerializeField] private CommandRecorder _commandRecorder;
        [SerializeField] private LosePanel _losePanel;
        
        [Inject] [SerializeField] private GameSession _gameSession;
        
        public override void InstallBindings()
        {
            Container.Bind<PlayerBehaviour>().FromInstance(_playerBehaviour).AsSingle().NonLazy();
            BindViewer();
            BindRecorder();
            BindLosePanel();
        }
        
        private void BindViewer()
        {
            Container.Bind<PlayerViewer>().FromInstance(_playerViewer).AsSingle().NonLazy();
        }

        private void BindRecorder()
        {
            Container.Bind<CommandRecorder>().FromInstance(_commandRecorder).AsSingle().NonLazy();
        }

        private void BindLosePanel()
        {
            Container.Bind<LosePanel>().FromInstance(_losePanel).AsSingle().NonLazy();
        }

        public void Initialize()
        {
        }
    }
}
