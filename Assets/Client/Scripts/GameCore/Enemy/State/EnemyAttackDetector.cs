using UnityEngine;
using UnityEngine.Events;

namespace Client
{
    public class EnemyAttackDetector : MonoBehaviour
    {
        public event UnityAction Entered;
        
        public event UnityAction DetectExited;

        public PlayerBehaviour PlayerTarget { get; private set; }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out PlayerBehaviour playerBehaviour)) return;
            PlayerTarget = playerBehaviour;
            Entered?.Invoke();
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.TryGetComponent(out PlayerBehaviour playerBehaviour)) return;
            DetectExited?.Invoke();
            PlayerTarget = null;
        }
    }
}