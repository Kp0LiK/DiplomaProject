using System.Threading.Tasks;
using Client.Scripts.Data.Enemy;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace Client.Scripts.GameCore.Enemy.Golem
{
    public class DragonSoulFollowState : BaseEnemyState
    {
        private readonly NavMeshAgent _navMeshAgent;
        private readonly EnemyPlayerDetector _playerDetector;
        private readonly EnemyData _enemyData;

        private bool _isScream;

        private static readonly int Run = Animator.StringToHash("Run");


        public DragonSoulFollowState(Animator animation, IEnemySwitchState enemySwitchState, NavMeshAgent navMeshAgent,
            EnemyPlayerDetector playerDetector, EnemyData enemydata)
            : base(animation, enemySwitchState)
        {
            _navMeshAgent = navMeshAgent;
            _playerDetector = playerDetector;
            _enemyData = enemydata;
        }

        public override async void Start()
        {
            if (!_isScream)
            {
                Animation.SetTrigger("Scream");
                await Task.Delay(1500);
            }
            _isScream = true;
            Animation.SetFloat(Run, 0.9f);
            _navMeshAgent.speed = _enemyData.Speed;
            _navMeshAgent.stoppingDistance = _enemyData.StopDistance;
            _navMeshAgent.isStopped = false;
        }

        public override void Stop()
        {
            Animation.SetFloat(Run, 0f);
            _navMeshAgent.isStopped = true;
        }

        public override async Task Action()
        {
            while (true)
            {
                await UniTask.Delay(2000);
                
                if (ReferenceEquals(_playerDetector.PlayerTarget, null))
                    return;

                if (ReferenceEquals(_navMeshAgent, null))
                    return;
                
                if (_navMeshAgent.isOnNavMesh)
                {
                    _navMeshAgent.SetDestination
                        (_playerDetector.PlayerTarget.transform.position);
                }
                else
                {
                    return;
                }

            }
        }
    }
}