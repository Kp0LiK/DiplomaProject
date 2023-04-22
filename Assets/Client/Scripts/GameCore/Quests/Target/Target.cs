using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] private string _name;

    public static Action<string> OnKilled;

    public void Die()
    {
        OnKilled?.Invoke(_name);
    }
}
