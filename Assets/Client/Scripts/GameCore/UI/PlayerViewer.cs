using System;
using System.Collections;
using System.Collections.Generic;
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
        [SerializeField] private Image _fillHealthImage;

        private PlayerBehaviour _playerBehaviour;

        [Inject]
        public void Constructor(PlayerBehaviour playerBehaviour)
        {
            _playerBehaviour = playerBehaviour;
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
    }
}
