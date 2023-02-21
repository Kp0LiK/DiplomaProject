using UnityEngine;
using UnityEngine.Events;

namespace Client
{
    public class EnemyPlayerDetector : MonoBehaviour
    {
        public event UnityAction<PlayerBehaviour> Entered;
        public event UnityAction<PlayerBehaviour> DetectExited;
        
        public PlayerBehaviour PlayerTarget { get; private set; }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out PlayerBehaviour playerBehaviour)) return;
            PlayerTarget = playerBehaviour;
            Entered?.Invoke(playerBehaviour);
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.TryGetComponent(out PlayerBehaviour playerBehaviour)) return;
            DetectExited?.Invoke(playerBehaviour);
            PlayerTarget = null;
        }
    }
}

