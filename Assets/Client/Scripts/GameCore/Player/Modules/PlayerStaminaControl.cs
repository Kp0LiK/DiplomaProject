using System.Collections;
using UnityEngine;

namespace Client
{
    [RequireComponent(typeof(PlayerBehaviour))]
    public class PlayerStaminaControl : MonoBehaviour
    {
        [SerializeField] private GameObject _healBuff;
        [SerializeField] private GameObject _healBuffAura;
        
        [SerializeField] private float _restoreDelay;
        [SerializeField] private float _restoreManaDelay;
        [SerializeField] private float _multiplayer;
        [SerializeField] private float _manaMultiplayer;
        
        private PlayerBehaviour _player;

        private Coroutine _healthRoutine;
        private Coroutine _restoreRoutine;
        private Coroutine _restoreManaRoutine;
        private WaitForSeconds _delay;
        private WaitForSeconds _manaDelay;

        private WaitForEndOfFrame _forEndOfFrame;

        private void Awake()
        {
            _forEndOfFrame = new WaitForEndOfFrame();
            _delay = new WaitForSeconds(_restoreDelay);
            _manaDelay = new WaitForSeconds(_restoreManaDelay);
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
            if (value < 100)
            {
               // _healthRoutine = StartCoroutine(HealthRestore(value));
            }
            else if (value >= 100)
            {
                _healBuff.SetActive(false);
                _healBuffAura.SetActive(false);
            }
            else
            {
                if (ReferenceEquals(_healthRoutine, null))
                    return;
                StopCoroutine(_healthRoutine);
                _healthRoutine = null;
            }
        }
        private void OnStaminaChange(float value)
        {
            if (value < 100)
            {
                _restoreRoutine = StartCoroutine(StaminaRestore(value));
            }
            else
            {
                if (ReferenceEquals(_restoreRoutine, null))
                    return;

                StopCoroutine(_restoreRoutine);
                _restoreRoutine = null;
            }
        }
        
        private void OnManaChanged(float value)
        {
            if (value < 100)
                _restoreManaRoutine = StartCoroutine(ManaRestore(value));
            else
            {
                if (ReferenceEquals(_restoreManaRoutine, null))
                    return;

                StopCoroutine(_restoreManaRoutine);
                _restoreManaRoutine = null;
            }
        }


        private IEnumerator HealthRestore(float currentValue)
        {
            if (currentValue >= 100)
            {
                StopCoroutine(_healthRoutine);
                yield return _forEndOfFrame;
            }
            yield return _delay;
            _healBuff.SetActive(true);
            _healBuffAura.SetActive(true);
            _player.Health += _multiplayer;
        }

        private IEnumerator StaminaRestore(float currentValue)
        {
            if (currentValue >= 100)
            {
                StopCoroutine(_restoreRoutine);
                yield return _forEndOfFrame;
            }
            
            yield return _delay;
            _player.Stamina += _multiplayer;
        }
        
        private IEnumerator ManaRestore(float currentValue)
        {
            if (currentValue >= 100)
            {
                StopCoroutine(_restoreManaRoutine);
                yield return _forEndOfFrame;
            }
            
            yield return _manaDelay;
            _player.Mana += _manaMultiplayer;
        }
        
    }
}