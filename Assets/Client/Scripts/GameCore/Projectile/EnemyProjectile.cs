using System;
using System.Collections;
using System.Collections.Generic;
using Client;
using UnityEngine;

public class EnemyProjectile : Projectile
{
    protected override void OnCollisionEnter(Collision other)
    {
        Instantiate(_collisionPrefab, transform.position, transform.rotation);
        AudioSource.PlayClipAtPoint(_collisionSound, gameObject.transform.position);
        Destroy(gameObject, 3);

        Rigidbody.isKinematic = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out PlayerBehaviour playerBehaviour))
        {
            Debug.Log("Player hit!");
            playerBehaviour.ApplyDamage(_weaponsData.Damage);
            Instantiate(_collisionPrefab, transform.position, transform.rotation);
            AudioSource.PlayClipAtPoint(_collisionSound, gameObject.transform.position);
            Destroy(gameObject);
        }
    }
}
