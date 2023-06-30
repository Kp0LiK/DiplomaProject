using System;
using System.Collections;
using System.Collections.Generic;
using Client;
using Client.Scripts.Data.Enemy;
using UnityEngine;

namespace Client
{
    public class EnemyProjectile : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private EnemyData _projectileData;
        [SerializeField] private ParticleSystem _collisionPrefab;
        [SerializeField] private AudioClip _collisionSound;

        public Rigidbody Rigidbody => _rigidbody;

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.TryGetComponent(out PlayerBehaviour player))
            {
                player.ApplyDamage(_projectileData.Damage);
            }

            Instantiate(_collisionPrefab, transform.position, transform.rotation);
            AudioSource.PlayClipAtPoint(_collisionSound, transform.position);
            Destroy(gameObject);
        }
    }
}