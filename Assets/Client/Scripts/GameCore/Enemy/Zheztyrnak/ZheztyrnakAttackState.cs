using System.Threading.Tasks;
using Client.Scripts.Data.Enemy;
using UnityEngine;

namespace Client
{
    public class ZheztyrnakAttackState : BaseEnemyState
    {
        private EnemyAttackDetector _enemyAttackDetector;
        private readonly EnemyData _enemyData;

        private ZheztyrnakBehaviour _zheztyrnakBehaviour;
        private static readonly int IsAttack = Animator.StringToHash("IsAttack");

        public ZheztyrnakAttackState(Animator animation, IEnemySwitchState enemySwitchState,
            EnemyAttackDetector enemyAttackDetector, EnemyData enemyData, ZheztyrnakBehaviour zheztyrnakBehaviour
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
                await Task.Delay(1300);

                if (_enemyData.IsDied)
                    return;

                if (_enemyAttackDetector.enabled != true) continue;
                if (!ReferenceEquals(_enemyAttackDetector.PlayerTarget, null))
                {
                    if (_enemyAttackDetector.PlayerTarget.gameObject.TryGetComponent
                            (out PlayerBehaviour playerBehaviour) &&
                        _enemyAttackDetector.PlayerTarget.IsStanding == false)
                    {
                        _enemyAttackDetector.PlayerTarget.ApplyDamage(_enemyData.Damage);
                        if (_zheztyrnakBehaviour.Health <= _enemyData.Health / 1.5f)
                        {
                            _enemyAttackDetector.PlayerTarget.ApplyDamage(_enemyData.Damage * 2f);
                        }
                        else
                        {
                            if (_zheztyrnakBehaviour.Health <= _enemyData.Health / 3f)
                            {
                                _enemyAttackDetector.PlayerTarget.ApplyDamage(_enemyData.Damage * 3f);
                            }
                            else
                            {
                                return;
                            }
                        }
                    }
                }
            }
        }
    }
}