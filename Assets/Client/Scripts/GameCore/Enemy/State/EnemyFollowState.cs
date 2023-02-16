using System.Threading.Tasks;
using Client.Scripts.Data.Enemy;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace Client
{
    public class EnemyFollowState : BaseEnemyState
    {
        private readonly NavMeshAgent _navMeshAgent;
        private readonly EnemyPlayerDetector _playerDetector;
        private readonly EnemyData _enemyData;


        public EnemyFollowState(IEnemySwitchState enemySwitchState, NavMeshAgent navMeshAgent,
            EnemyPlayerDetector playerDetector, EnemyData enemydata) : base(enemySwitchState)
        {
            _navMeshAgent = navMeshAgent;
            _playerDetector = playerDetector;
            _enemyData = enemydata;
        }

        public override void Start()
        {
            _navMeshAgent.speed = _enemyData.Speed;
            _navMeshAgent.stoppingDistance = _enemyData.StopDistance;
            _navMeshAgent.isStopped = false;
        }

        public override void Stop()
        {
            _navMeshAgent.isStopped = true;
        }

        public override async Task Action()
        {
            while (true)
            {
                await UniTask.Delay(1);
                
                if (ReferenceEquals(_playerDetector.PlayerTarget, null))
                    return;

                if (ReferenceEquals(_navMeshAgent, null))
                    return;
                
                if (_navMeshAgent.isOnNavMesh) _navMeshAgent.SetDestination
                    (_playerDetector.PlayerTarget.transform.position);
                else return;
            }
        }
    }
}

