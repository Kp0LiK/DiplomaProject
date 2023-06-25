using System.Threading.Tasks;
using UnityEngine;

namespace Client
{
    public class GiantIdleState : BaseEnemyState
    {
        private static readonly int Idle = Animator.StringToHash("Idle");

        public GiantIdleState(Animator animator, IEnemySwitchState enemySwitchState) : base(animator, enemySwitchState)
        {
            
        }

        public override void Start()
        {
            Animation.SetTrigger(Idle);
        }

        public override void Stop()
        {
            
        }

        public override async Task Action()
        {
            
        }
    }
}

