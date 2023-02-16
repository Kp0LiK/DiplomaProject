using System.Threading.Tasks;
using UnityEngine;

namespace Client
{
    public abstract class BaseEnemyState : MonoBehaviour
    {
        public readonly IEnemySwitchState EnemySwitchState;

        protected BaseEnemyState(IEnemySwitchState enemySwitchState)
        {
            EnemySwitchState = enemySwitchState;
        }

        public abstract void Start();
        public abstract void Stop();
        public abstract Task Action();
    }
}

