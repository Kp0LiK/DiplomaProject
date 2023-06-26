using System.Threading.Tasks;
using Client.Scripts.Data.Enemy;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace Client.Scripts.GameCore.Enemy.Golem
{
    public class DragonSoulFlyState : BaseEnemyState
    {
        private readonly NavMeshAgent _navMeshAgent;
        private readonly EnemyPlayerDetector _playerDetector;
        private readonly EnemyData _enemyData;

        private bool _isFly;

        private static readonly int FlyForward = Animator.StringToHash("FlyForward");
        private static readonly int Fly = Animator.StringToHash("Fly");


        public DragonSoulFlyState(Animator animation, IEnemySwitchState enemySwitchState, NavMeshAgent navMeshAgent,
            EnemyPlayerDetector playerDetector, EnemyData enemydata)
            : base(animation, enemySwitchState)
        {
            _navMeshAgent = navMeshAgent;
            _playerDetector = playerDetector;
            _enemyData = enemydata;
        }

        public override async void Start()
        {
            if (!_isFly)
            {
                Animation.SetTrigger(Fly);
                await Task.Delay(1500);
            }

            _isFly = true;
            Animation.SetFloat(FlyForward, 1f);
            _navMeshAgent.speed = _enemyData.Speed;
            _navMeshAgent.stoppingDistance = _enemyData.StopDistance;
            _navMeshAgent.isStopped = false;
        }

        public override void Stop()
        {
            Animation.SetFloat(FlyForward, 0f);
            _navMeshAgent.isStopped = true;
        }

        public override async Task Action()
        {
            while (true)
            {
                await UniTask.Delay(1500);
                
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