using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Client
{
    public class LosePanel : MonoBehaviour
    {
        [SerializeField] private Button _resetButton;
        [SerializeField] private Button _quitButton;
        [SerializeField] private GameObject _object;


        private GameSession _gameSession;
        private SceneLoader _sceneLoader;
        private CommandRecorder _commandRecorder;

        
        [Inject]
        public void Constructor(GameSession gameSession, SceneLoader sceneLoader, CommandRecorder commandRecorder)
        {
            _gameSession = gameSession;
            _sceneLoader = sceneLoader;
            _commandRecorder = commandRecorder;
        }

        private void Start() => _object.SetActive(false);

        private void OnEnable()
        {
            _resetButton.onClick.AddListener(OnResetButton);
            _quitButton.onClick.AddListener(OnQuitPress);

        }

        private void OnDisable()
        {
            _resetButton.onClick.RemoveListener(OnResetButton);
            _quitButton.onClick.RemoveListener(OnQuitPress);
        }
        
        private void OnResetButton()
        {
            _commandRecorder.Rewind();
            _sceneLoader.LoadSceneAsync("Menu");
        }

        private void OnQuitPress()
        {
           Application.Quit();
        }
    }
}

