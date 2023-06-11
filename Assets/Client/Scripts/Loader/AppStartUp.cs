using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Client
{
    public class AppStartUp : MonoBehaviour
    {
        private Queue<ILoadOperation> _operations;

        private SceneLoader _sceneLoader;
        private GameSession _gameSession;


        [Inject]
        public void Constructor(SceneLoader sceneLoader, GameSession gameSession)
        {
            _sceneLoader = sceneLoader;
            _gameSession = gameSession;
        }

        private void Awake()
        {
            _operations = new Queue<ILoadOperation>();

            _operations.Enqueue(new GameSettingLoader(_gameSession));
        }

        private void Start()
        {
            Load(_operations);
        }

        private async void Load(Queue<ILoadOperation> loadingOperations)
        {
            foreach (var operation in loadingOperations)
            {
                await Task.Delay(500);
                await operation.Load(OnProgress);
            }

            _sceneLoader.LoadSceneAsync("Menu");
        }

        private void OnProgress()
        {
        }
    }
}