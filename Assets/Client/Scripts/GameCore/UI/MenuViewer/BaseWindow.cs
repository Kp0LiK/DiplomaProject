using UnityEngine;
using UnityEngine.UI;

namespace Client
{
    public class BaseWindow : MonoBehaviour
    {
        [field: SerializeField] public  BaseWindowManager Manager { get; private set; }
        [SerializeField] private Button _backButton;

        protected virtual void Awake() => Manager = GetComponentInParent<BaseWindowManager>();

        protected virtual void OnEnable()
        {
            if (ReferenceEquals(_backButton, null))
                return;

            _backButton.onClick.AddListener(Back);
        }

        protected virtual void OnDisable()
        {
            if (ReferenceEquals(_backButton, null))
                return;
            
            _backButton.onClick.RemoveListener(Back);
        }

        private void Back()
        {
            Manager.BackWindow();
        }

        public void Open()
        {
            Manager.CloseAllWindows();
            gameObject.SetActive(true);
        }

        public void Close() => gameObject.SetActive(false);
    }
}

