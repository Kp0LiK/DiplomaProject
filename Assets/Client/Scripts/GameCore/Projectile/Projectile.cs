using System.Collections;
using System.Collections.Generic;
using Client.Scripts.Data.Player;
using UnityEngine;

namespace Client
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private WeaponsData _weaponsData;
        [SerializeField] private ParticleSystem _collisionPrefab;

        public Rigidbody Rigidbody => _rigidbody;
        
        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.TryGetComponent(out IDamageable damageable))
            {
                damageable.ApplyDamage(_weaponsData.Damage);
                Instantiate(_collisionPrefab, transform.position, transform.rotation);
                Destroy(gameObject);
            }
            else
            {
                Instantiate(_collisionPrefab, transform.position, transform.rotation);
                Destroy(gameObject, 3);
            }

            Rigidbody.isKinematic = true;
        }
    }
}