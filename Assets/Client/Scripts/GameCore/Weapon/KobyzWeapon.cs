using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KobyzWeapon : MonoBehaviour
{
    [field: SerializeField] public Transform CastPoint { get; private set; }
    
    private bool _isCanCast;
    
    private void Start()
    {
        _isCanCast = true;
    }
}
