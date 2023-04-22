using System;
using System.Collections;
using System.Collections.Generic;
using Client;
using UnityEngine;

public class WeaponBehaviour : MonoBehaviour
{
    public bool Collidable;
    private bool _collided;
    private IDamageable _enemy;

    public bool Collided => _collided;
    public IDamageable Enemy => _enemy;
    
    private void OnTriggerEnter(Collider other)
    {
        if (Collidable)
        {
            if (_collided == false && other.TryGetComponent(out IDamageable enemy))
            {
                _enemy = enemy;
                _collided = true;
                Enemy.ApplyDamage(5);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out IDamageable enemy))
        {
            _enemy = null;
            _collided = false;
            Collidable = false;
        }
    }
}