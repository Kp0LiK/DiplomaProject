using System;
using System.Threading.Tasks;
using Client.Scripts.Data.Enemy;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Client
{
    public class DragonSoulAttackState : BaseEnemyState
    {
        private EnemyAttackDetector _enemyAttackDetector;
        private readonly EnemyData _enemyData;

        private DragonSoulBehaviour _dragonSoulBehaviour;
        private readonly Func<EnemyProjectile> _projectileFactory;
        private readonly Action<EnemyProjectile> _projectileDestroyCallback;
        private static readonly int IsBasicAttack = Animator.StringToHash("IsBasicAttack");
        private static readonly int IsFireballAttack = Animator.StringToHash("IsFireballAttack");

        private bool _fireballAttack;

        public DragonSoulAttackState(Animator animation, IEnemySwitchState enemySwitchState,
            EnemyAttackDetector enemyAttackDetector, EnemyData enemyData, DragonSoulBehaviour dragonSoulBehaviour,
            Func<EnemyProjectile> projectileFactory, Action<EnemyProjectile> projectileDestroyCallback
        ) : base(animation, enemySwitchState)
        {
            _enemyAttackDetector = enemyAttackDetector;
            _enemyData = enemyData;
            _dragonSoulBehaviour = dragonSoulBehaviour;
            _projectileFactory = projectileFactory;
            _projectileDestroyCallback = projectileDestroyCallback;
        }

        public override void Start()
        {
            int randomAnimation = Random.Range(0, 2);

            switch (randomAnimation)
            {
                case 0:
                    Animation.SetBool(IsBasicAttack, true);
                    _fireballAttack = false;
                    break;
                case 1:
                    Animation.SetBool(IsFireballAttack, true);
                    _fireballAttack = true;
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
                    if (_fireballAttack)
                    {
                        if (_enemyAttackDetector.PlayerTarget.gameObject.TryGetComponent<PlayerBehaviour>(out _)
                            && _enemyAttackDetector.PlayerTarget.IsStanding == false)
                        {
                            projectile = _projectileFactory.Invoke();
                            projectile.transform.position = new Vector3(position.x, position.y + 2.8f, position.z +3f);
                            projectile.transform.rotation = _enemyAttackDetector.transform.rotation;
                            _enemyAttackDetector.PlayerTarget.ApplyDamage(_enemyData.Damage * 2f);
                        }

                        if (projectile != null)
                        {
                            projectile.Rigidbody.velocity = _enemyAttackDetector.transform.forward * 5f;
                            _projectileDestroyCallback?.Invoke(projectile);
                        }
                    }
                    else
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