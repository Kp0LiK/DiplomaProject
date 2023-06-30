using System;
using System.Threading.Tasks;
using Client.Scripts.Data.Enemy;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Client
{
    public class DragonTerrorAttackState : BaseEnemyState
    {
        private EnemyAttackDetector _enemyAttackDetector;
        private readonly EnemyData _enemyData;

        private DragonTerrorBehaviour _dragonTerrorBehaviour;
        private static readonly int IsBasicAttack = Animator.StringToHash("IsBasicAttack");
        private static readonly int IsFireballAttack = Animator.StringToHash("IsClawAttack");
        public DragonTerrorAttackState(Animator animation, IEnemySwitchState enemySwitchState,
            EnemyAttackDetector enemyAttackDetector, EnemyData enemyData, DragonTerrorBehaviour dragonTerrorBehaviour
        ) : base(animation, enemySwitchState)
        {
            _enemyAttackDetector = enemyAttackDetector;
            _enemyData = enemyData;
            _dragonTerrorBehaviour = dragonTerrorBehaviour;
        }

        public override void Start()
        {
            int randomAnimation = Random.Range(0, 2);

            switch (randomAnimation)
            {
                case 0:
                    Animation.SetBool(IsBasicAttack, true);
                    break;
                case 1:
                    Animation.SetBool(IsFireballAttack, true);
                    break;
            }
        }

        public override void Stop()
        {
            Animation.SetBool(IsBasicAttack, false);
            Animation.SetBool(IsFireballAttack, false);
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
                        if (_dragonTerrorBehaviour.Health <= _enemyData.Health / 1.5f)
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