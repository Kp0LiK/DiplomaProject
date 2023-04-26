using System;
using Client.Scripts.Data.Player;
using UnityEngine;

namespace Client
{
    public class BowWeapon : MonoBehaviour
    {
        [SerializeField] private Arrow _arrowPrefab;
        [SerializeField] private float _arrowSpeed;
        [field: SerializeField] public Transform ShootPoint { get; private set; }
        [field: SerializeField] public ParticleSystem ShootParticlePrefab { get; private set; }

        private bool _isCanShoot;

        private void Start()
        {
            _isCanShoot = true;
        }
        

        public void Shoot()
        {
            if (!_isCanShoot)
            {
                return;
            }
            
            if (!ReferenceEquals(ShootParticlePrefab, null))
            {
                var effect = Instantiate(ShootParticlePrefab, ShootPoint.position, ShootPoint.rotation);
                Destroy(effect.gameObject, effect.main.duration * 2f);
            }
            
            var ray = new Ray(ShootPoint.position, ShootPoint.forward);
            if (Physics.Raycast(ray, out var hit, Mathf.Infinity))
            {
                if (hit.transform.TryGetComponent(out PlayerBehaviour _))
                    return;
            }
            
            var arrow = Instantiate(_arrowPrefab, ShootPoint.position, ShootPoint.transform.rotation);
            var velocity = (hit.point);
            arrow.Rigidbody.velocity = ShootPoint.forward * _arrowSpeed;
            Destroy(arrow.gameObject, 2);
        }
    }
}