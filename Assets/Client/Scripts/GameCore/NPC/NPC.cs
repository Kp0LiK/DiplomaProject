using System;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public Action Interacted;
    public Action<bool> Approached;

    private bool _interactable;
    public bool Interactable => _interactable;

    private void OnEnable()
    {
        Interacted += Talk;
        Approached += OnApproach;
    }

    private void OnDisable()
    {
        Interacted -= Talk;
        Approached -= OnApproach;
    }

    protected virtual void Talk()
    {
        Debug.Log("Hello, Traveler.");
    }

    protected virtual void OnApproach(bool interactable)
    {
        _interactable = interactable;
    }
}
