using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Zenject;

namespace Client
{
    public class StartWindow : BaseWindow
    {
        [SerializeField] private Button _startButton;
        [SerializeField] private Button _exitButton;
        [SerializeField] private Button[] _buttons;
        private SceneLoader _sceneLoader;

        [Inject]
        public void Constructor(SceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader;
        }

        protected override void OnEnable()
        {
            SignButtons(
                Manager.OpenWindow<SettingWindow>,
                Manager.OpenWindow<AboutWindow>
            );
            
            _startButton.onClick.AddListener(OnPressStartButton);
            _exitButton.onClick.AddListener(OnPressExitButton);
        }

        private void SignButtons(params UnityAction[] actions)
        {
            for (var i = 0; i < _buttons.Length; i++)
                _buttons[i].onClick.AddListener(actions[i]);
        }

        protected override void OnDisable()
        {
            foreach (var button in _buttons)
                button.onClick.RemoveAllListeners();
            
            _startButton.onClick.RemoveListener(OnPressStartButton);
            _exitButton.onClick.RemoveListener(OnPressExitButton);
        }

        private void OnPressStartButton()
        {
            _sceneLoader.LoadSceneAsync("TutorialScene");
        }

        private static void OnPressExitButton()
        {
            Application.Quit();
        }
    }
}