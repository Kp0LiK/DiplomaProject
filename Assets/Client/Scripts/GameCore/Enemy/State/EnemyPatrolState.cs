using System.Threading.Tasks;
using Client.Scripts.Data.Enemy;
using UnityEngine;
using UnityEngine.AI;

namespace Client
{
    public class EnemyPatrolState : BaseEnemyState
    {
        private readonly NavMeshAgent _navMeshAgent;
        private readonly EnemyData _enemyData;
        private static readonly int Run = Animator.StringToHash("Run");
        private readonly PatrolPoint[] _patrolPoints;
        private readonly int _initialWaitTime = 4;
        private float _waitTime;
        private int _currentWaypointIndex;

        public EnemyPatrolState(Animator animator, IEnemySwitchState enemySwitchState, 
            EnemyPatrolPointDetector patrolPointDetector, NavMeshAgent navMeshAgent, 
            EnemyData enemyData, PatrolPoint[] patrolPoints) : base(animator, enemySwitchState)
        {
            _navMeshAgent = navMeshAgent;
            _enemyData = enemyData;
            _patrolPoints = patrolPoints;
        }

        public override void Start()
        {
            Animation.SetFloat(Run, 1f);
            _navMeshAgent.speed = _enemyData.Speed;
            _navMeshAgent.stoppingDistance = _enemyData.StopDistance;
            _navMeshAgent.isStopped = false;
            _waitTime = _initialWaitTime;
        }

        public override void Stop()
        {
            Animation.SetFloat(Run, 0f);
        }

        public override async Task Action()
        {
            Patrol();
        }

        private void Patrol()
        {
            _waitTime = _initialWaitTime;
            Animation.SetFloat(Run, 1f);
            RandomIndex();
            _navMeshAgent.SetDestination(_patrolPoints[_currentWaypointIndex].transform.position);
            if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
            {
                if (_waitTime <= 0)
                {
                    VisitNextPoint();
                    _waitTime = _initialWaitTime;
                }
                else
                {
                    Stop();
                    _waitTime -= Time.deltaTime;
                }
            }
        }

        private void RandomIndex()
        {
            int previousIndex = _currentWaypointIndex;
            _currentWaypointIndex = Random.Range(0, _patrolPoints.Length);

            if (_currentWaypointIndex == previousIndex)
            {
                RandomIndex();
            }
        }

        private void VisitNextPoint()
        {
            RandomIndex();
            _navMeshAgent.SetDestination(_patrolPoints[_currentWaypointIndex].transform.position);
        }
    }
}