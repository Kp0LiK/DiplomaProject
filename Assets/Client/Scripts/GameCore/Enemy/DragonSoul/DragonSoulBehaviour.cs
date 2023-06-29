using System;
using System.Collections.Generic;
using System.Linq;
using Client.Scripts.Data.Enemy;
using Client.Scripts.GameCore.Enemy.Golem;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;

namespace Client
{
    [SelectionBase]
    public class DragonSoulBehaviour : MonoBehaviour, IEnemySwitchState, IDamageable
    {
        [SerializeField, Required] private EnemyData _enemyData;
        [SerializeField] private float _deathDuration = 2f;
        [SerializeField] private DragonSoulAudioData _audioData;
        [SerializeField] private EnemyProjectile _fireballPrefab;

        public event Action<float> HealthChanged;
        public float Health { get; private set; }

        private EnemyPlayerDetector _playerDetector;
        private EnemyAttackDetector _enemyAttackDetector;
        private PlayerBehaviour _target;
        private Animator _animator;
        private AudioSource _audioSource;
        private bool _isDead;
        private Target _questTarget;

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
            _enemyAttackDetector = GetComponentInChildren<EnemyAttackDetector>();
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _audioSource = GetComponent<AudioSource>();
            _questTarget = GetComponent<Target>();
        }

        private void Start()
        {
            Health = _enemyData.Health;
            _enemyData.IsDied = false;

            _states = new List<BaseEnemyState>
            {
                new EnemyIdleState(_animator, this),
                new DragonSoulFollowState(_animator, this, _navMeshAgent, _playerDetector, _enemyData),
                new DragonSoulFlyState(_animator, this, _navMeshAgent, _playerDetector, _enemyData),
                new DragonSoulAttackState(_animator, this, _enemyAttackDetector, _enemyData, this),
                new DragonSoulFlyAttackState(_animator, this, _enemyAttackDetector, _enemyData, _fireballPrefab),
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

        private void OnEntered(PlayerBehaviour arg0)
        {
            SwitchState<DragonSoulFollowState>();
            if (Health <= _enemyData.Health / 2)
            {
                SwitchState<DragonSoulFlyState>();
                _audioSource.PlayOneShot(_audioData.OnFly);
            }
            _audioSource.PlayOneShot(_audioData.OnDetect);
            
            PlayerBehaviour.OnEncounter?.Invoke();
        }

        private void OnDetectExited(PlayerBehaviour arg0)
        {
            SwitchState<EnemyIdleState>();
            _audioSource.PlayOneShot(_audioData.OnUnDetect);
        }

        private void OnSpiderAttackDetect()
        {
            _audioSource.PlayOneShot(_audioData.OnHit);
            SwitchState<DragonSoulAttackState>();
            if (Health <= _enemyData.Health / 2)
            {
                SwitchState<DragonSoulFlyAttackState>();
            }
        }

        private void OnAttackDetectExited()
        {
            SwitchState<DragonSoulFollowState>();
            if (Health <= _enemyData.Health / 2)
            {
                _audioSource.PlayOneShot(_audioData.OnFly);
                SwitchState<DragonSoulFlyState>();
            }
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
        public class DragonSoulAudioData
        {
            public AudioClip OnDetect;
            public AudioClip OnUnDetect;
            public AudioClip OnHit;
            public AudioClip OnFly;
            public AudioClip OnDie;
        }
    }
}