using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTalker : MonoBehaviour
{
    private bool _isNearTalker;
    private Talker _currentTalker;

    private void Update()
    {
        if (_isNearTalker && Input.GetKeyDown(KeyCode.E))
        {
            _currentTalker.Talk();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Talker talker))
        {
            _isNearTalker = true;
            _currentTalker = talker;
        }
    }
}
