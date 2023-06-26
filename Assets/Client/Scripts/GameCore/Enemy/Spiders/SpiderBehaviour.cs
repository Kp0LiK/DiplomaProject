using System;
using System.Collections.Generic;
using System.Linq;
using Client.Scripts.Data.Enemy;
using ModestTree;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Client
{
    [SelectionBase]
    public class SpiderBehaviour : MonoBehaviour, IEnemySwitchState, IDamageable
    {
        [SerializeField, Required] private EnemyData _enemyData;
        [SerializeField] private float _deathDuration = 2f;
        [SerializeField] private PatrolPoint[] _patrolPoints;
        [SerializeField] private SpiderAudioData _audioData;

        public event Action<float> HealthChanged;
        public static Action OnDeath;
        public float Health { get; private set; }

        private EnemyPlayerDetector _playerDetector;
        private EnemyPatrolPointDetector _patrolPointDetector;
        private EnemyAttackDetector _enemyAttackDetector;
        private PlayerBehaviour _target;
        private Animator _animator;
        private Target _questTarget;
        private AudioSource _audioSource;
        private bool _isDead;

        private NavMeshAgent _navMeshAgent;
        private Rigidbody _rigidbody;
        private List<BaseEnemyState> _states;
        public BaseEnemyState CurrentState { get; private set; }
        public PlayerBehaviour Target => _target;

        public EnemyData Data => _enemyData;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _rigidbody = GetComponent<Rigidbody>();
            _playerDetector = GetComponentInChildren<EnemyPlayerDetector>();
            _patrolPointDetector = GetComponentInChildren<EnemyPatrolPointDetector>();
            _enemyAttackDetector = GetComponentInChildren<EnemyAttackDetector>();
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _questTarget = GetComponent<Target>();
            _audioSource = GetComponent<AudioSource>();
        }

        private void Start()
        {
            Health = _enemyData.Health;
            _enemyData.IsDied = false;
            Debug.Log(Health);

            _states = new List<BaseEnemyState>
            {
                new EnemyIdleState(_animator, this),
                new EnemyFollowState(_animator, this, _navMeshAgent, _playerDetector, _enemyData),
                new SpiderAttackState(_animator, this, _enemyAttackDetector, _enemyData),
                new EnemyDeathState(_animator, this, _playerDetector, _enemyAttackDetector, _navMeshAgent),
                new EnemyPatrolState(_animator, this, _patrolPointDetector, _navMeshAgent, _enemyData, _patrolPoints)
            };

            CurrentState = _states[4];
            CurrentState.Start();
            CurrentState.Action();
        }

        private void OnEnable()
        {
            _playerDetector.Entered += OnEntered;
            _playerDetector.DetectExited += OnDetectExited;

            _patrolPointDetector.Entered += OnPatrolPointEntered;

            _enemyAttackDetector.Entered += OnSpiderAttackDetect;
            _enemyAttackDetector.DetectExited += OnAttackDetectExited;
        }

        private void OnDisable()
        {
            _playerDetector.Entered -= OnEntered;
            _playerDetector.DetectExited -= OnDetectExited;

            _patrolPointDetector.Entered -= OnPatrolPointEntered;

            _enemyAttackDetector.Entered -= OnSpiderAttackDetect;
            _enemyAttackDetector.DetectExited -= OnAttackDetectExited;
        }

        private void OnDestroy() => CurrentState.Stop();

        private void OnEntered(PlayerBehaviour arg0)
        {
            SwitchState<EnemyFollowState>();
            _audioSource.PlayOneShot(_audioData.OnDetect);
            PlayerBehaviour.OnEncounter?.Invoke();
        }

        private void OnDetectExited(PlayerBehaviour arg0)
        {
            SwitchState<EnemyPatrolState>();
            _audioSource.PlayOneShot(_audioData.OnUnDetect);
        }

        private void OnPatrolPointEntered(PatrolPoint patrolPoint)
        {
            switch (CurrentState)
            {
                case EnemyPatrolState _:
                    CurrentState.Action();
                    break;
            }
        }

        private void OnSpiderAttackDetect()
        {
            SwitchState<SpiderAttackState>();
            _audioSource.PlayOneShot(_audioData.OnHit[Random.Range(0, 3)]);
        }

        private void OnAttackDetectExited()
        {
            SwitchState<EnemyFollowState>();
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

            if (Health <= 0 && !_isDead)
            {
                Health = 0;
                SwitchState<EnemyDeathState>();
                OnDeath?.Invoke();
                _questTarget.Die();
                _audioSource.PlayOneShot(_audioData.OnDie);
                _isDead = true;
                Destroy(gameObject, _deathDuration);
                //_enemyData.IsDied = true;
            }

            if (_enemyData.IsDied)
            {
            }

            HealthChanged?.Invoke(Health);
        }

        private void SpiderDamageAnimation()
        {
            if (ReferenceEquals(gameObject, null))
                return;
            //todo DamageAnimation
        }

        [Serializable]
        public class SpiderAudioData
        {
            public AudioClip OnDetect;
            public AudioClip OnUnDetect;
            public AudioClip[] OnHit;
            public AudioClip OnDie;
        }
    }
}