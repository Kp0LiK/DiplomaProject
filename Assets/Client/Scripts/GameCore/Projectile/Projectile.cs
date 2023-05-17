using System;
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
        [SerializeField] private AudioClip _collisionSound;

        public Rigidbody Rigidbody => _rigidbody;

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.TryGetComponent(out IDamageable damageable))
            {
                damageable.ApplyDamage(_weaponsData.Damage);
                Instantiate(_collisionPrefab, transform.position, transform.rotation);
                AudioSource.PlayClipAtPoint(_collisionSound, gameObject.transform.position);
                Destroy(gameObject);
            }
            else
            {
                Instantiate(_collisionPrefab, transform.position, transform.rotation);
                AudioSource.PlayClipAtPoint(_collisionSound, gameObject.transform.position);
                Destroy(gameObject, 3);
            }

            Rigidbody.isKinematic = true;
        }
    }
}