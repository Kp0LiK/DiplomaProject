using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Client
{
    public class StartPanel : MonoBehaviour
    {
        [SerializeField] private Button _yesButton;
        [SerializeField] private Button _noButton;

        private SceneLoader _sceneLoader;

        [Inject]
        public void Constructor(SceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader;
        }
        private void OnEnable()
        {
            _yesButton.onClick.AddListener(OnYesButtonClick);
            _noButton.onClick.AddListener(OnNoButtonClick);
        }

        private void OnDisable()
        {
            _yesButton.onClick.RemoveListener(OnYesButtonClick);
            _noButton.onClick.RemoveListener(OnNoButtonClick);
        }

        private void OnYesButtonClick()
        {
            _sceneLoader.LoadSceneAsync("FullGame");
        }
        
        private void OnNoButtonClick()
        {
            _sceneLoader.LoadSceneAsync("TutorialScene");
        }
    }

}