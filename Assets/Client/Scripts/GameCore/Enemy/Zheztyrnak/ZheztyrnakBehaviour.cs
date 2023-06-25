using System;
using System.Collections.Generic;
using System.Linq;
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

        public event Action<float> HealthChanged;
        public float Health { get; private set; }

        private EnemyPlayerDetector _playerDetector;
        private EnemyAttackDetector _enemyAttackDetector;
        private PlayerBehaviour _target;
        private Animator _animator;

        private NavMeshAgent _navMeshAgent;
        private Rigidbody _rigidbody;
        private List<BaseEnemyState> _states;

        private int _attackCounter = 0;
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
        }

        private void Start()
        {
            Health = _enemyData.Health;
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

        private void OnEntered(PlayerBehaviour arg0)
        {
            SwitchState<ZheztyrnakDistantAttackState>();
        }

        private void OnDetectExited(PlayerBehaviour arg0)
        {
            SwitchState<EnemyIdleState>();
        }

        private void OnSpiderAttackDetect()
        {
            SwitchState<ZheztyrnakAttackState>();
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

            if (Health <= 70f)
            {
                _scale.transform.DOScale(new Vector3(2f, 2f, 2f), 3f);
            }

            if (Health <= _enemyData.Health / 2f)
            {
                _scale.transform.DOScale(new Vector3(2.5f, 2.5f, 2.5f), 3f);
            }

            if (Health <= 0)
            {
                Health = 0;
                SwitchState<EnemyDeathState>();
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
    }
}