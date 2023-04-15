using System;
using System.Collections;
using System.Collections.Generic;
using Client;
using UnityEngine;

public class WeaponBehaviour : MonoBehaviour
{
    public bool Collidable;
    private bool _collided;
    private SpiderBehaviour _spiderBehaviour;

    public bool Collided => _collided;
    public SpiderBehaviour SpiderBehaviour => _spiderBehaviour;
    
    private void OnTriggerEnter(Collider other)
    {
        if (Collidable)
        {
            if (_collided == false && other.TryGetComponent(out SpiderBehaviour spiderBehaviour))
            {
                _spiderBehaviour = spiderBehaviour;
                _collided = true;
                SpiderBehaviour.ApplyDamage(5);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out SpiderBehaviour spiderBehaviour))
        {
            _spiderBehaviour = null;
            _collided = false;
            Collidable = false;
        }
    }
}