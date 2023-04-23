using System;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField] private string[] _lines;
    
    public Action Interacted;
    public Action<bool> Approached;
    public static Action<string[]> DialogueStarted;

    private bool _interactable;
    public bool Interactable => _interactable;

    private bool _talking;

    protected virtual void OnEnable()
    {
        Interacted += Talk;
        Approached += OnApproach;
        DialogueSystem.DialogueEnded += StopTalking;
    }

    protected virtual void OnDisable()
    {
        Interacted -= Talk;
        Approached -= OnApproach;
        DialogueSystem.DialogueEnded -= StopTalking;
    }

    protected virtual void Talk()
    {
        if (!_talking)
        {
            DialogueStarted?.Invoke(_lines);
            _talking = true;
        }
    }

    protected virtual void OnApproach(bool interactable)
    {
        _interactable = interactable;
    }

    private void StopTalking()
    {
        _talking = false;
    }
}
