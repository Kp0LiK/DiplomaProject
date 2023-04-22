using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    [SerializeField] private string _collectibleName;

    public static Action<string> OnCollected;

    public void Collect()
    {
        OnCollected?.Invoke(_collectibleName);
    }
}