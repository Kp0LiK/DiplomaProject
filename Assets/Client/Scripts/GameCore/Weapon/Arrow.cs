using System;
using Client.Scripts.Data.Player;
using UnityEngine;

namespace Client
{
    public class Arrow : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private WeaponsData _weaponsData;

        public Rigidbody Rigidbody => _rigidbody;
        
        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.TryGetComponent(out IDamageable damageable))
            {
                damageable.ApplyDamage(_weaponsData.Damage);
                Destroy(gameObject);
            }
            else
            {
                Destroy(gameObject, 3);
            }

            Rigidbody.isKinematic = true;
        }
    }
}

