using System.Threading.Tasks;
using Client;
using UnityEngine;
using UnityEngine.AI;

public class GolemDeathState : BaseEnemyState
{
    private static readonly int IsDie = Animator.StringToHash("IsDie");

    private EnemyPlayerDetector _enemyPlayerDetector;
    private EnemyAttackDetector _enemyAttackDetector;
    private NavMeshAgent _navMeshAgent;

    public GolemDeathState(Animator animation, IEnemySwitchState enemySwitchState,
        EnemyPlayerDetector enemyPlayerDetector, EnemyAttackDetector enemyAttackDetector,
        NavMeshAgent navMeshAgent) : base(animation, enemySwitchState)
    {
        _enemyAttackDetector = enemyAttackDetector;
        _enemyPlayerDetector = enemyPlayerDetector;
        _navMeshAgent = navMeshAgent;
    }

    public override void Start()
    {
        Animation.SetBool(IsDie, true);
        _enemyAttackDetector.enabled = false;
        _enemyPlayerDetector.enabled = false;
        _navMeshAgent.enabled = false;
        Object.Destroy(_navMeshAgent.gameObject, 5);
    }

    public override void Stop()
    {
        Animation.SetBool(IsDie, false);
    }

    public override async Task Action()
    {
       
    }
}