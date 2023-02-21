using System.Threading.Tasks;
using UnityEngine;

namespace Client
{
    public abstract class BaseEnemyState
    {
        protected readonly Animator Animation;
        public readonly IEnemySwitchState EnemySwitchState;

        protected BaseEnemyState(Animator animation, IEnemySwitchState enemySwitchState)
        {
            Animation = animation;
            EnemySwitchState = enemySwitchState;
        }

        public abstract void Start();
        public abstract void Stop();
        public abstract Task Action();
    }
}

