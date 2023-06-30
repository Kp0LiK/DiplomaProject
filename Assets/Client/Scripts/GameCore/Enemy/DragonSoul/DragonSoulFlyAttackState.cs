using System;
using System.Threading.Tasks;
using Client.Scripts.Data.Enemy;
using UnityEngine;

namespace Client
{
    public class DragonSoulFlyAttackState : BaseEnemyState
    {
        private EnemyAttackDetector _enemyAttackDetector;
        private readonly EnemyData _enemyData;
        private readonly Func<EnemyProjectile> _projectileFactory;
        private readonly Action<EnemyProjectile> _projectileDestroyCallback;

        private static readonly int IsFlyAttack = Animator.StringToHash("IsFlyAttack");

        public DragonSoulFlyAttackState(Animator animation, IEnemySwitchState enemySwitchState,
            EnemyAttackDetector enemyAttackDetector, EnemyData enemyData, Func<EnemyProjectile> projectileFactory,
            Action<EnemyProjectile> projectileDestroyCallback)
            : base(animation, enemySwitchState)
        {
            _enemyAttackDetector = enemyAttackDetector;
            _enemyData = enemyData;
            _projectileFactory = projectileFactory;
            _projectileDestroyCallback = projectileDestroyCallback;
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
            EnemyProjectile projectile = null;
            while (true)
            {
                await Task.Delay(1000);

                Vector3 position = _enemyAttackDetector.transform.position;

                if (_enemyData.IsDied)
                    return;

                if (_enemyAttackDetector.enabled != true) continue;
                if (!ReferenceEquals(_enemyAttackDetector.PlayerTarget, null))
                {
                    if (_enemyAttackDetector.PlayerTarget.gameObject.TryGetComponent<PlayerBehaviour>(out var playerBehaviour) &&
                        _enemyAttackDetector.PlayerTarget.IsStanding == false)
                    {
                        projectile = _projectileFactory.Invoke();
                        projectile.transform.position = new Vector3(position.x, position.y + 7f, position.z);
                        projectile.transform.rotation = _enemyAttackDetector.transform.rotation;
                        _enemyAttackDetector.PlayerTarget.ApplyDamage(_enemyData.Damage);
                    }

                    if (projectile != null)
                    {
                        projectile.Rigidbody.velocity = _enemyAttackDetector.transform.forward * 5f;
                        _projectileDestroyCallback?.Invoke(projectile);
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