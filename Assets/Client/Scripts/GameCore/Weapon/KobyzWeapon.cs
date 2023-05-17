using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Client
{
    public class KobyzWeapon : MonoBehaviour
    {
        [SerializeField] private float _projectileSpeed;
        [field: SerializeField] public Transform CastPoint { get; private set; }

        private AudioSource _audioSource;

        [Header("Projectiles")]
        [SerializeField] private Projectile _fireballPrefab;
        [SerializeField] private Projectile _iceballPrefab;

        private bool _isCanCast;

        private void Start()
        {
            _isCanCast = true;
            _audioSource = GetComponent<AudioSource>();
        }

        public void Shoot(ProjectileType projectileType)
        {
            if (!_isCanCast) return;
            
            _audioSource.Play();
            var ray = new Ray(CastPoint.position, CastPoint.forward);
            if (Physics.Raycast(ray, out var hit, Mathf.Infinity))
            {
                if (hit.transform.TryGetComponent(out PlayerBehaviour _)) return;
            }

            Projectile projectile = null;
            
            switch (projectileType)
            {
                case ProjectileType.FIREBALL:
                    projectile = Instantiate(_fireballPrefab, CastPoint.position, CastPoint.transform.rotation);
                    break;
                case ProjectileType.ICE:
                    projectile = Instantiate(_iceballPrefab, CastPoint.position, CastPoint.transform.rotation);
                    break;
            }

            projectile.Rigidbody.velocity = CastPoint.forward * _projectileSpeed;
            Destroy(projectile.gameObject, 2);
        }
    }
}