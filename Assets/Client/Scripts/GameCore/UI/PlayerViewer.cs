using System;
using System.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Client
{
    public class PlayerViewer : MonoBehaviour
    {
        [SerializeField, BoxGroup("Sliders")] private Slider _healthViewer;
        [SerializeField, BoxGroup("Sliders")] private Slider _staminaViewer;
        [SerializeField, BoxGroup("Sliders")] private Slider _manaViewer;
        
        [SerializeField, BoxGroup("Images")] private Image _healthImage;
        [SerializeField, BoxGroup("Images")] private Image _manaImage;
        [SerializeField, BoxGroup("Images")] private Image _energyImage;
        
        [SerializeField, BoxGroup("Sprites")] private Sprite[] _healthSprites;
        [SerializeField, BoxGroup("Sprites")] private Sprite[] _manaSprites;
        [SerializeField, BoxGroup("Sprites")] private Sprite[] _energySprites;

        [SerializeField] private Canvas _inGameMenu;
        [SerializeField] private Canvas _inGameLose;

        private PlayerBehaviour _playerBehaviour;
        private CommandRecorder _commandRecorder;
        public BaseCommand _inGameMenuCommand;
        public BaseCommand _inGameLoseCommand;


        [Inject]
        public void Constructor(PlayerBehaviour playerBehaviour, CommandRecorder commandRecorder)
        {
            _playerBehaviour = playerBehaviour;
            _commandRecorder = commandRecorder;
        }

        private void Awake()
        {
            _inGameMenuCommand = new InGameMenuCommand(_inGameMenu, GetComponent<Canvas>());
            _inGameLoseCommand = new InGameMenuCommand(_inGameLose, GetComponent<Canvas>());
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                OnPauseButton();
            }
        }

        private void OnEnable()
        {
            _playerBehaviour.HealthChanged += OnHealthChanged;
            _playerBehaviour.StaminaChanged += OnStaminaChanged;
            _playerBehaviour.ManaChanged += OnManaChanged;
        }

        private void OnDisable()
        {
            _playerBehaviour.HealthChanged -= OnHealthChanged;
            _playerBehaviour.StaminaChanged -= OnStaminaChanged;
            _playerBehaviour.ManaChanged -= OnManaChanged;
        }

        private async void OnHealthChanged(float health)
        {
            _healthViewer.DOValue(health, 0.5f);
            int spriteIndex = Mathf.Clamp((int)(health / 20f), 0, _healthSprites.Length - 1);
            _healthImage.sprite = _healthSprites[spriteIndex];
            
            if (health <= 0)
            {
                await Task.Delay(4500);
                _playerBehaviour.gameObject.SetActive(false);
                OnLoseButton();
            }
        }

        private void OnStaminaChanged(float energy)
        {
            _staminaViewer.DOValue(energy, 0.5f);
            int spriteIndex = Mathf.Clamp((int)(energy / 20f), 0, _energySprites.Length - 1);
            _energyImage.sprite = _energySprites[spriteIndex];
            if (energy <= 0)
            {
                //todo HealSystem
            }
        }
        
        private void OnManaChanged(float mana)
        {
            _manaViewer.DOValue(mana, 0.5f);
            int spriteIndex = Mathf.Clamp((int)(mana / 20f), 0, _manaSprites.Length - 1);
            _manaImage.sprite = _manaSprites[spriteIndex];
            if (mana <= 0)
            {
                //todo HealSystem
            }
        }
        
        private void OnPauseButton() => _commandRecorder.Record(_inGameMenuCommand);
        private void OnLoseButton() => _commandRecorder.Record(_inGameLoseCommand);

    }
}
