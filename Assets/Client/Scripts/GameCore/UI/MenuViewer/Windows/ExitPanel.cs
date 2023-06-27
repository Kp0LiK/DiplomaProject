using UnityEngine;
using UnityEngine.UI;

namespace Client
{
    public class ExitPanel : MonoBehaviour
    {
        [SerializeField] private Button _yesButton;
        [SerializeField] private Button _noButton;
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
            Application.Quit();
        }
        
        private void OnNoButtonClick()
        {
            gameObject.SetActive(false);
        }
    }

}