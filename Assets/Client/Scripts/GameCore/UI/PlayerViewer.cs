using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Client
{
    public class PlayerViewer : MonoBehaviour
    {
        [SerializeField] private Slider _healthViewer;
        [SerializeField] private Slider _staminaViewer;
        [SerializeField] private Slider _manaViewer;
        
        [SerializeField] private Canvas _inGameMenu;

        private PlayerBehaviour _playerBehaviour;
        private CommandRecorder _commandRecorder;
        private BaseCommand _inGameMenuCommand;


        [Inject]
        public void Constructor(PlayerBehaviour playerBehaviour, CommandRecorder commandRecorder)
        {
            _playerBehaviour = playerBehaviour;
            _commandRecorder = commandRecorder;
        }

        private void Awake()
        {
            _inGameMenuCommand = new InGameMenuCommand(_inGameMenu, GetComponent<Canvas>());
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

        private void OnHealthChanged(float health)
        {
            _healthViewer.DOValue(health, 0.5f);
            // if (health <= 0)
            // {
            //     _fillHealthImage.DOFade(0, 0.5f);
            // }
        }

        private void OnStaminaChanged(float energy)
        {
            _staminaViewer.DOValue(energy, 0.5f);
            if (energy <= 0)
            {
                //todo HealSystem
            }
        }
        
        private void OnManaChanged(float mana)
        {
            _manaViewer.DOValue(mana, 0.5f);
            if (mana <= 0)
            {
                //todo HealSystem
            }
        }
        
        private void OnPauseButton() => _commandRecorder.Record(_inGameMenuCommand);

    }
}
