using System.Threading.Tasks;
using Client.Scripts.Data.Enemy;
using UnityEngine;

namespace Client
{
    public class BoarNewAttackState : BaseEnemyState
    {
        private EnemyAttackDetector _enemyAttackDetector;
        private readonly EnemyData _enemyData;
        
        private static readonly int IsAttack = Animator.StringToHash("IsSecondAttack");

        public BoarNewAttackState(Animator animation, IEnemySwitchState enemySwitchState,
            EnemyAttackDetector enemyAttackDetector,  EnemyData enemyData
        ) : base(animation, enemySwitchState)
        {
            _enemyAttackDetector = enemyAttackDetector;
            _enemyData = enemyData;
        }

        public override void Start()
        {
            Animation.SetBool(IsAttack, true);
        }

        public override void Stop()
        {
            Animation.SetBool(IsAttack, false);

        }

        public override async Task Action()
        {
            while (true)
            {
                await Task.Delay(1200);

                if (_enemyData.IsDied)
                    return;

                if (_enemyAttackDetector.enabled != true) continue;
                if (!ReferenceEquals(_enemyAttackDetector.PlayerTarget, null))
                {
                    if (_enemyAttackDetector.PlayerTarget.gameObject.TryGetComponent<PlayerBehaviour>(
                            out var playerBehaviour)
                        && _enemyAttackDetector.PlayerTarget.IsStanding == false)
                    {
                        _enemyAttackDetector.PlayerTarget.ApplyDamage(_enemyData.Damage * 2f);
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