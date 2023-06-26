using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Client.Scripts.Data.Enemy;
using Client.Scripts.GameCore.Enemy.Golem;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;


namespace Client
{
    [SelectionBase]
    public class ZheztyrnakBehaviour : MonoBehaviour, IEnemySwitchState, IDamageable
    {
        [SerializeField, Required] private EnemyData _enemyData;
        [SerializeField] private float _deathDuration = 2f;
        [SerializeField] private GameObject _scale;
        [SerializeField] private ZheztyrnakAudioData _audioData;

        public event Action<float> HealthChanged;
        public float Health { get; private set; }

        private EnemyPlayerDetector _playerDetector;
        private EnemyAttackDetector _enemyAttackDetector;
        private PlayerBehaviour _target;
        private Animator _animator;
        private AudioSource _audioSource;
        private string _name;
        private bool _scaledOnce;
        private bool _scaledTwice;

        private NavMeshAgent _navMeshAgent;
        private Rigidbody _rigidbody;
        private List<BaseEnemyState> _states;

        private int _attackCounter = 0;
        private bool _isDie;
        public BaseEnemyState CurrentState { get; private set; }
        public PlayerBehaviour Target => _target;

        public EnemyData Data => _enemyData;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _rigidbody = GetComponent<Rigidbody>();
            _playerDetector = GetComponentInChildren<EnemyPlayerDetector>();
            _enemyAttackDetector = GetComponentInChildren<EnemyAttackDetector>();
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _audioSource = GetComponent<AudioSource>();
            _name = "Zheztyrnak";
        }

        private void Start()
        {
            Health = _enemyData.Health;
            BossHPViewer.OnHealthInitialized?.Invoke(Health);
            _enemyData.IsDied = false;

            _states = new List<BaseEnemyState>
            {
                new EnemyIdleState(_animator, this),
                new ZheztyrnakAttackState(_animator, this, _enemyAttackDetector, _enemyData, this),
                new ZheztyrnakDistantAttackState(_animator, this, _enemyAttackDetector, _enemyData, this),
                new EnemyDeathState(_animator, this, _playerDetector, _enemyAttackDetector, _navMeshAgent)
            };

            CurrentState = _states[0];
            CurrentState.Start();
            CurrentState.Action();
        }

        private void OnEnable()
        {
            _playerDetector.Entered += OnEntered;
            _playerDetector.DetectExited += OnDetectExited;


            _enemyAttackDetector.Entered += OnSpiderAttackDetect;
            _enemyAttackDetector.DetectExited += OnAttackDetectExited;
        }

        private void OnDisable()
        {
            _playerDetector.Entered -= OnEntered;
            _playerDetector.DetectExited -= OnDetectExited;


            _enemyAttackDetector.Entered -= OnSpiderAttackDetect;
            _enemyAttackDetector.DetectExited -= OnAttackDetectExited;
        }

        private void OnDestroy() => CurrentState.Stop();

        private async void OnEntered(PlayerBehaviour arg0)
        {
            SwitchState<ZheztyrnakDistantAttackState>();
            BossHPViewer.OnBossEnter?.Invoke(_name);
            
            _audioSource.PlayOneShot(_audioData.OnTrigger);
            Time.timeScale = 0.05f;
            await Task.Delay(10000);
            Time.timeScale = 1;
            _audioSource.PlayOneShot(_audioData.OnEnter);
        }

        private void OnDetectExited(PlayerBehaviour arg0)
        {
            SwitchState<EnemyIdleState>();
        }

        private void OnSpiderAttackDetect()
        {
            SwitchState<ZheztyrnakAttackState>();
            _audioSource.PlayOneShot(_audioData.OnHit);
            _attackCounter++;
        }

        private void OnAttackDetectExited()
        {
            SwitchState<ZheztyrnakDistantAttackState>();
        }

        public void SwitchState<T>() where T : BaseEnemyState
        {
            var state = _states.FirstOrDefault(p => p is T);
            CurrentState.Stop();
            CurrentState = state;

            if (ReferenceEquals(CurrentState, null))
                return;

            CurrentState.Start();
            CurrentState.Action();
        }

        public void ApplyDamage(float damage)
        {
            Health -= damage;

            if (Health <= 70f && !_scaledOnce)
            {
                _scale.transform.DOScale(new Vector3(2f, 2f, 2f), 3f);
                _audioSource.PlayOneShot(_audioData.OnTaunt);
                _scaledOnce = true;
            }

            if (Health <= _enemyData.Health / 2f && !_scaledTwice)
            {
                _scale.transform.DOScale(new Vector3(2.5f, 2.5f, 2.5f), 3f);
                _audioSource.PlayOneShot(_audioData.OnTaunt2);
                _scaledTwice = true;
            }

            if (Health <= 0 && !_isDie)
            {
                _isDie = true;
                Health = 0;
                SwitchState<EnemyDeathState>();
                _audioSource.PlayOneShot(_audioData.OnDie);
                BossHPViewer.OnBossDeath?.Invoke();
                _scale.transform.DOScale(new Vector3(0.5f, 0.5f, 0.5f), 7f);
                Destroy(gameObject, _deathDuration);
            }

            if (_enemyData.IsDied)
            {
            }

            HealthChanged?.Invoke(Health);
            BossHPViewer.OnHealthChanged?.Invoke(Health);
        }

        private void SpiderDamageAnimation()
        {
            if (ReferenceEquals(gameObject, null))
                return;
            //todo DamageAnimation
        }
        
        [Serializable]
        public class ZheztyrnakAudioData
        {
            public AudioClip OnEnter;
            public AudioClip OnTrigger;
            public AudioClip OnHit;
            public AudioClip OnDie;
            public AudioClip OnTaunt;
            public AudioClip OnTaunt2;
        }
    }
}