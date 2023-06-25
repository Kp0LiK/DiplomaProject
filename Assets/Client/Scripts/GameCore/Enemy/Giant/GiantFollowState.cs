using System.Threading.Tasks;
using Client.Scripts.Data.Enemy;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace Client.Scripts.GameCore.Enemy.Golem
{
    public class GiantFollowState : BaseEnemyState
    {
        private readonly NavMeshAgent _navMeshAgent;
        private readonly EnemyPlayerDetector _playerDetector;
        private readonly EnemyData _enemyData;

        private GiantBehaviour _giantBehaviour;
        private static readonly int Run = Animator.StringToHash("Run");


        public GiantFollowState(Animator animation, IEnemySwitchState enemySwitchState, NavMeshAgent navMeshAgent,
            EnemyPlayerDetector playerDetector, EnemyData enemydata, GiantBehaviour boarBehaviour)
            : base(animation, enemySwitchState)
        {
            _navMeshAgent = navMeshAgent;
            _playerDetector = playerDetector;
            _enemyData = enemydata;
            _giantBehaviour = boarBehaviour;
        }

        public override async void Start()
        {
            Animation.SetTrigger("Sleep");
            await Task.Delay(1000);
            Animation.SetTrigger("WakeUp");
            await Task.Delay(1500);
            Animation.SetFloat(Run, 0.5f);
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
                    if (_giantBehaviour.Health <= _enemyData.Health / 2f)
                    {
                        _navMeshAgent.speed = _enemyData.Speed * 3f;
                        Animation.SetFloat(Run, 1f);
                    }
                }
                else
                {
                    return;
                }

            }
        }
    }
}