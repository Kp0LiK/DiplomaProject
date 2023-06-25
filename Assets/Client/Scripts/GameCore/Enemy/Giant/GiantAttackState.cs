using System.Threading.Tasks;
using Client.Scripts.Data.Enemy;
using UnityEngine;

namespace Client
{
    public class GiantAttackState : BaseEnemyState
    {
        private EnemyAttackDetector _enemyAttackDetector;
        private readonly EnemyData _enemyData;

        private GiantBehaviour _giantBehaviour;
        private static readonly int IsAttack = Animator.StringToHash("IsAttack");
        private static readonly int IsComboAttack = Animator.StringToHash("IsComboAttack");
        private static readonly int IsJumpAttack = Animator.StringToHash("IsJumpAttack");

        public GiantAttackState(Animator animation, IEnemySwitchState enemySwitchState,
            EnemyAttackDetector enemyAttackDetector, EnemyData enemyData, GiantBehaviour giantBehaviour
        ) : base(animation, enemySwitchState)
        {
            _enemyAttackDetector = enemyAttackDetector;
            _enemyData = enemyData;
        }

        public override void Start()
        {
            int randomAnimation = Random.Range(0, 3);

            switch (randomAnimation)
            {
                case 0:
                    Animation.SetBool(IsAttack, true);
                    break;
                case 1:
                    Animation.SetBool(IsComboAttack, true);
                    break;
                case 2:
                    Animation.SetBool(IsJumpAttack, true);
                    break;
            }
        }

        public override void Stop()
        {
            Animation.SetBool(IsAttack, false);
            Animation.SetBool(IsComboAttack, false);
            Animation.SetBool(IsJumpAttack, false);
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
                        if (_giantBehaviour.Health <= _enemyData.Health / 1.5f)
                        {
                            _enemyAttackDetector.PlayerTarget.ApplyDamage(_enemyData.Damage * 2f);
                        }
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