using System.Threading.Tasks;
using Client.Scripts.Data.Enemy;
using UnityEngine;

namespace Client
{
    public class DragonSoulFlyAttackState : BaseEnemyState
    {
        private EnemyAttackDetector _enemyAttackDetector;
        private readonly EnemyData _enemyData;
        private Projectile _fireBallPrefab;

        private static readonly int IsFlyAttack = Animator.StringToHash("IsFlyAttack");

        public DragonSoulFlyAttackState(Animator animation, IEnemySwitchState enemySwitchState,
            EnemyAttackDetector enemyAttackDetector, EnemyData enemyData, Projectile fireBallPrefab
        ) : base(animation, enemySwitchState)
        {
            _enemyAttackDetector = enemyAttackDetector;
            _enemyData = enemyData;
            _fireBallPrefab = fireBallPrefab;
        }

        public override void Start()
        {
            Animation.SetBool(IsFlyAttack, true);
        }

        public override void Stop()
        {
            Animation.SetBool(IsFlyAttack, false);
        }

        public override async Task Action()
        {
            Projectile projectile = null;
            while (true)
            {
                await Task.Delay(1300);

                Vector3 position = _enemyAttackDetector.transform.position;

                if (_enemyData.IsDied)
                    return;

                if (_enemyAttackDetector.enabled != true) continue;
                if (!ReferenceEquals(_enemyAttackDetector.PlayerTarget, null))
                {
                    if (_enemyAttackDetector.PlayerTarget.gameObject.TryGetComponent
                            (out PlayerBehaviour playerBehaviour) &&
                        _enemyAttackDetector.PlayerTarget.IsStanding == false)
                    {
                        projectile = Instantiate(_fireBallPrefab, new Vector3(position.x, position.y + 7f, position.z), _enemyAttackDetector.transform.rotation);
                        // _enemyAttackDetector.PlayerTarget.ApplyDamage(_enemyData.Damage);
                    }
                    
                    projectile.Rigidbody.velocity = _enemyAttackDetector.transform.forward * 5f;
                    Destroy(projectile.gameObject, 2);
                }
                else
                {
                    return;
                }
            }
        }
    }
}