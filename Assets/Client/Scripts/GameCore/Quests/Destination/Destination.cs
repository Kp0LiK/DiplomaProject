using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destination : MonoBehaviour
{
    [SerializeField] private string _name;

    public static Action<string> OnReached;

    public void Reach()
    {
        OnReached?.Invoke(_name);
    }
}
