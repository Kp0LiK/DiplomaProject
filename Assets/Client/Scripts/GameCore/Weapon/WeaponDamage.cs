using System;
using Client.Scripts.Data.Player;
using UnityEngine;

namespace Client
{
    public class WeaponDamage : MonoBehaviour
    {
        [SerializeField] private WeaponsData _weaponsData;

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.TryGetComponent(out IDamageable damageable))
            {
                damageable.ApplyDamage(_weaponsData.Damage);
                Destroy(gameObject);
            }
        }
    }
}

