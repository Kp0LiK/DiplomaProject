using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Talker : MonoBehaviour
{
    [SerializeField] private string _name;

    public static Action<string> OnTalked;

    public void Talk()
    {
        OnTalked?.Invoke(_name);
    }
}
