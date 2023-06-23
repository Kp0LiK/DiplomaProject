using System.Collections;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Client
{
    public class PauseViewer : MonoBehaviour
    {
        [SerializeField, BoxGroup("Buttons")] private Button _continueButton;
        [SerializeField, BoxGroup("Buttons")] private Button _settingButton;
        [SerializeField, BoxGroup("Buttons")] private Button _helpButton;
        [SerializeField, BoxGroup("Buttons")] private Button _exitMenuButton;
        [SerializeField, BoxGroup("Buttons")] private Button _exitYesButton;
        [SerializeField, BoxGroup("Buttons")] private Button _exitNoButton;

        [SerializeField, BoxGroup("Transform")] private Transform _settingButtonTransform;
        [SerializeField, BoxGroup("Transform")] private Transform _helpButtonTransform;
        [SerializeField, BoxGroup("Transform")] private Transform _exitButtonTransform;

        [SerializeField, BoxGroup("Panels")] private GameObject _settingPanel;
        [SerializeField, BoxGroup("Panels")] private GameObject _helpPanel;
        [SerializeField, BoxGroup("Panels")] private GameObject _exitPanel;

        private CommandRecorder _commandRecorder;
        private GameSession _gameSession;
        private SceneLoader _sceneLoader;
        
        private const float AnimationDuration = 0.3f;
        private bool _isAnimating = false;

        [Inject]
        public void Constructor(CommandRecorder commandRecorder, SceneLoader sceneLoader, GameSession gameSession)
        {
            _commandRecorder = commandRecorder;
            _gameSession = gameSession;
            _sceneLoader = sceneLoader;
        }

        private void Start() => gameObject.SetActive(false);

        private void OnEnable()
        {
            _continueButton.onClick.AddListener(OnContinueButton);
            _settingButton.onClick.AddListener(OnSettingButton);
            _helpButton.onClick.AddListener(OnHelpButton);
            _exitMenuButton.onClick.AddListener(OnMainMenuButton);
            
            _exitYesButton.onClick.AddListener(OnExitYesButton);
            _exitNoButton.onClick.AddListener(OnExitNoButton);
        }

        private void OnDisable()
        {
            _continueButton.onClick.RemoveListener(OnContinueButton);
            _settingButton.onClick.RemoveListener(OnSettingButton);
            _helpButton.onClick.RemoveListener(OnHelpButton);
            _exitMenuButton.onClick.RemoveListener(OnMainMenuButton);
            
            _exitYesButton.onClick.RemoveListener(OnExitYesButton);
            _exitNoButton.onClick.RemoveListener(OnExitNoButton);
        }

        private void OnContinueButton()
        {
            _commandRecorder.Rewind();

            var returnPivotX = 0.5f;
            SetButtonPivotX(_settingButtonTransform, returnPivotX);
            SetButtonPivotX(_helpButtonTransform, returnPivotX);
            SetButtonPivotX(_exitButtonTransform, returnPivotX);

            _settingButtonTransform.transform.DOScale(new Vector3(1f, 1f, 1f), 0.5f);
            _helpButtonTransform.transform.DOScale(new Vector3(1f, 1f, 1f), 0.5f);
            _exitButtonTransform.transform.DOScale(new Vector3(1f, 1f, 1f), 0.5f);

            _exitPanel.gameObject.SetActive(false);
            _settingPanel.gameObject.SetActive(false);
            _helpPanel.gameObject.SetActive(false);
        }

        private void OnSettingButton()
        {
            if (_isAnimating)
                return;
            
            _isAnimating = true;
            
            float newPivotX = 0f;
            float oldPivotX = 0.5f;
            SetButtonPivotX(_settingButtonTransform, newPivotX);
            SetButtonPivotX(_helpButtonTransform, oldPivotX);
            SetButtonPivotX(_exitButtonTransform, oldPivotX);
            
            StartCoroutine(AnimateButtons(_settingButtonTransform, new Vector3(1.2f, 1.2f, 1f)));
            StartCoroutine(AnimateButtons(_helpButtonTransform, new Vector3(1f, 1f, 1f)));
            StartCoroutine(AnimateButtons(_exitButtonTransform, new Vector3(1f, 1f, 1f)));

            _settingPanel.gameObject.SetActive(true);
            _helpPanel.gameObject.SetActive(false);
            _exitPanel.gameObject.SetActive(false);
        }

        private void OnHelpButton()
        {
            if (_isAnimating)
                return;
            
            _isAnimating = true;
            
            float newPivotX = 0f;
            float oldPivotX = 0.5f;
            SetButtonPivotX(_helpButtonTransform, newPivotX);
            SetButtonPivotX(_exitButtonTransform, oldPivotX);
            SetButtonPivotX(_settingButtonTransform, oldPivotX);
            
            StartCoroutine(AnimateButtons(_helpButtonTransform, new Vector3(1.2f, 1.2f, 1f)));
            StartCoroutine(AnimateButtons(_settingButtonTransform, new Vector3(1f, 1f, 1f)));
            StartCoroutine(AnimateButtons(_exitButtonTransform, new Vector3(1f, 1f, 1f)));

            _helpPanel.gameObject.SetActive(true);
            _settingPanel.gameObject.SetActive(false);
            _exitPanel.gameObject.SetActive(false);
        }

        private void OnMainMenuButton()
        {
            if (_isAnimating)
                return;
            
            _isAnimating = true;
            
            float newPivotX = 0f;
            float oldPivotX = 0.5f;
            SetButtonPivotX(_exitButtonTransform, newPivotX);
            SetButtonPivotX(_helpButtonTransform, oldPivotX);
            SetButtonPivotX(_settingButtonTransform, oldPivotX);

            StartCoroutine(AnimateButtons(_exitButtonTransform, new Vector3(1.2f, 1.2f, 1f)));
            StartCoroutine(AnimateButtons(_settingButtonTransform, new Vector3(1f, 1f, 1f)));
            StartCoroutine(AnimateButtons(_helpButtonTransform, new Vector3(1f, 1f, 1f)));

            _exitPanel.gameObject.SetActive(true);
            _settingPanel.gameObject.SetActive(false);
            _helpPanel.gameObject.SetActive(false);
        }

        private void OnExitYesButton()
        {
            _commandRecorder.Rewind();
            _sceneLoader.LoadSceneAsync("Menu");
        }

        private void OnExitNoButton()
        {
            _exitPanel.gameObject.SetActive(false);
            _settingPanel.gameObject.SetActive(false);
            _helpPanel.gameObject.SetActive(false);
        }
        
        private IEnumerator AnimateButtons(Transform buttonTransform, Vector3 targetScale)
        {
            Vector3 initialScale = buttonTransform.localScale;
            float elapsedTime = 0f;
            float animationTime = AnimationDuration;

            while (elapsedTime < animationTime)
            {
                float t = elapsedTime / animationTime;
                Vector3 newScale = Vector3.Lerp(initialScale, targetScale, t);
                
                buttonTransform.localScale = newScale;
                
                elapsedTime += Time.unscaledDeltaTime;

                yield return null;
            }
            
            buttonTransform.localScale = targetScale;

            _isAnimating = false;
        }
        
        private void SetButtonPivotX(Transform buttonTransform, float pivotX)
        {
            RectTransform buttonRectTransform = buttonTransform.GetComponent<RectTransform>();
            Vector2 pivot = buttonRectTransform.pivot;
            pivot.x = pivotX;
            buttonRectTransform.pivot = pivot;
        }
    }
}