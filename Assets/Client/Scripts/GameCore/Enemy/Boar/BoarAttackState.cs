using System.Threading.Tasks;
using Client.Scripts.Data.Enemy;
using UnityEngine;

namespace Client
{
    public class BoarAttackState : BaseEnemyState
    {
        private EnemyAttackDetector _enemyAttackDetector;
        private readonly EnemyData _enemyData;
        
        private BoarBehaviour _boarBehaviour;
        private static readonly int IsAttack = Animator.StringToHash("IsAttack");

        public BoarAttackState(Animator animation, IEnemySwitchState enemySwitchState,
            EnemyAttackDetector enemyAttackDetector,  EnemyData enemyData, BoarBehaviour boarBehaviour
        ) : base(animation, enemySwitchState)
        {
            _enemyAttackDetector = enemyAttackDetector;
            _enemyData = enemyData;
            _boarBehaviour = boarBehaviour;
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
                await Task.Delay(800);
                
                if (_enemyData.IsDied)
                    return;

                if (_enemyAttackDetector.enabled != true) continue;
                if (!ReferenceEquals(_enemyAttackDetector.PlayerTarget, null))
                {
                    if (_enemyAttackDetector.PlayerTarget.gameObject.TryGetComponent
                            (out PlayerBehaviour playerBehaviour) && _enemyAttackDetector.PlayerTarget.IsStanding == false)
                    {
                        _enemyAttackDetector.PlayerTarget.ApplyDamage(_enemyData.Damage);
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