using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;

namespace Client
{
    public class PlayerTeleporter : MonoBehaviour
    {
        [SerializeField] private PlayerBehaviour _playerBehaviour;
        
        [SerializeField] private Transform _teleporter;

        [SerializeField] private GameObject _firstLevel;
        [SerializeField] private GameObject _secondLevel;


        private async void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.TryGetComponent(out PlayerBehaviour playerBehaviour)) return;
            _firstLevel.gameObject.SetActive(false);
            _secondLevel.gameObject.SetActive(true);
            await Task.Delay(1000);
            _playerBehaviour.transform.position = _teleporter.transform.position;
        }
    }
}

