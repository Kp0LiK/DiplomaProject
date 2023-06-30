using System.Collections;
using UnityEngine;

namespace Client
{
        [RequireComponent(typeof(PlayerBehaviour))]
    public class PlayerModulesController : MonoBehaviour
    {
        [SerializeField] private GameObject _healBuff;
        [SerializeField] private GameObject _healBuffAura;

        [SerializeField] private float _restoreDelay;
        [SerializeField] private float _restoreManaDelay;
        [SerializeField] private float _restoreStaminaDelay;
        [SerializeField] private float _multiplayer;
        [SerializeField] private float _manaMultiplayer;
        [SerializeField] private float _staminaMultiplayer;

        private PlayerBehaviour _player;

        private Coroutine _healthRoutine;
        private Coroutine _restoreRoutine;
        private Coroutine _restoreManaRoutine;
        private WaitForSeconds _delay;
        private WaitForSeconds _manaDelay;
        private WaitForSeconds _staminaDelay;

        private void Awake()
        {
            _delay = new WaitForSeconds(_restoreDelay);
            _manaDelay = new WaitForSeconds(_restoreManaDelay);
            _staminaDelay = new WaitForSeconds(_restoreStaminaDelay);
            _player = GetComponent<PlayerBehaviour>();
        }

        private void Start()
        {
            _healBuff.SetActive(false);
            _healBuffAura.SetActive(false);
        }

        private void OnEnable()
        {
            _player.HealthChanged += OnHealthChange;
            _player.StaminaChanged += OnStaminaChange;
            _player.ManaChanged += OnManaChanged;
        }

        private void OnDisable()
        {
            _player.HealthChanged -= OnHealthChange;
            _player.StaminaChanged -= OnStaminaChange;
            _player.ManaChanged -= OnManaChanged;
        }

        private void OnHealthChange(float value)
        {
            if (value < 50 && ReferenceEquals(_healthRoutine, null))
            {
                _healthRoutine = StartCoroutine(HealthRestore());
            }
            else if (value >= 100)
            {
                StopCoroutine(_healthRoutine);
                _healthRoutine = null;
                _healBuff.SetActive(false);
                _healBuffAura.SetActive(false);
            }
        }

        private void OnStaminaChange(float value)
        {
            if (value < 100 && ReferenceEquals(_restoreRoutine, null))
            {
                _restoreRoutine = StartCoroutine(StaminaRestore());
            }
            else if (value >= 100)
            {
                StopCoroutine(_restoreRoutine);
                _restoreRoutine = null;
            }
        }

        private void OnManaChanged(float value)
        {
            if (value < 100 && ReferenceEquals(_restoreManaRoutine, null))
            {
                _restoreManaRoutine = StartCoroutine(ManaRestore());
            }
            else if (value >= 100)
            {
                StopCoroutine(_restoreManaRoutine);
                _restoreManaRoutine = null;
            }
        }

        private IEnumerator HealthRestore()
        {
            while (_player.Health < 100)
            {
                _healBuff.SetActive(true);
                _healBuffAura.SetActive(true);
                yield return _delay;
                _healBuff.SetActive(false);
                _healBuffAura.SetActive(false);
                _player.Health += _multiplayer;
            }
        }

        private IEnumerator StaminaRestore()
        {
            while (_player.Stamina < 100)
            {
                yield return _staminaDelay;
                _player.Stamina += _staminaMultiplayer;
            }
        }

        private IEnumerator ManaRestore()
        {
            while (_player.Mana < 100)
            {
                yield return _manaDelay;
                _player.Mana += _manaMultiplayer;
            }
        }
    }
}