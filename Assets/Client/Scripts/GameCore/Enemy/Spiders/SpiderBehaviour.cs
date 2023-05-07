using System;
using System.Collections.Generic;
using System.Linq;
using Client.Scripts.Data.Enemy;
using ModestTree;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;

namespace Client
{
    [SelectionBase]
    public class SpiderBehaviour : MonoBehaviour, IEnemySwitchState, IDamageable
    {
        [SerializeField, Required] protected EnemyData _enemyData;
        [SerializeField] protected float _deathDuration = 2f;
        [SerializeField] private PatrolPoint[] _patrolPoints;

        public event Action<float> HealthChanged;
        public static Action OnDeath;
        public float Health { get; protected set; }

        private EnemyPlayerDetector _playerDetector;
        private EnemyPatrolPointDetector _patrolPointDetector;
        private EnemyAttackDetector _enemyAttackDetector;
        private PlayerBehaviour _target;
        private Animator _animator;

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
        }

        private void Start()
        {
            Health = _enemyData.Health;
            _enemyData.IsDied = false;

            _states = new List<BaseEnemyState>
            {
                new EnemyIdleState(_animator, this),
                new EnemyFollowState(_animator, this, _navMeshAgent, _playerDetector, _enemyData),
                new SpiderAttackState(_animator, this, _enemyAttackDetector, _enemyData),
                new SpiderDeathState(_animator, this, _playerDetector, _enemyAttackDetector, _navMeshAgent),
                new EnemyPatrolState(_animator, this, _patrolPointDetector, _navMeshAgent, _enemyData, _patrolPoints)
            };

            CurrentState = _states[4];
            CurrentState.Start();
            CurrentState.Action();

            Debug.Log(Health);
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
        }

        private void OnDetectExited(PlayerBehaviour arg0)
        {
            SwitchState<EnemyPatrolState>();
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
            Debug.Log("Attack");
        }

        private void OnAttackDetectExited()
        {
            SwitchState<EnemyFollowState>();
        }

        private void Update()
        {
            // switch (CurrentState)
            // {
            //     case EnemyIdleState _:
            //         CurrentState.Action();
            //         break;
            // }
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

        public virtual void ApplyDamage(float damage)
        {
            Health -= damage;
                
            if (Health <= 0)
            {
                Health = 0;
                _enemyData.IsDied = true;
            }
            
            if (_enemyData.IsDied)
            {
                SwitchState<SpiderDeathState>();
                OnDeath?.Invoke();
                Destroy(gameObject, _deathDuration);
            }
            
            // if (Health <= 0)
            // {
            //     Health = 0;
            //     _enemyData.IsDied = true;
            // }
            //
            // if (_enemyData.IsDied)
            // {
            //     SwitchState<SpiderDeathState>();
            //     Destroy(gameObject, _deathDuration);
            // }
            // else
            // {
            //     Health -= damage;
            //     
            //     if (Health <= 0)
            //     {
            //         Health = 0;
            //         _enemyData.IsDied = true;
            //     }
            //     //SpiderDamageAnimation();
            // }

            HealthChanged?.Invoke(Health);
        }

        private void SpiderDamageAnimation()
        {
            if (ReferenceEquals(gameObject, null))
                return;
            //todo DamageAnimation
        }
    }
}