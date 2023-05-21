using System;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField] protected string[] _lines;
    private Animator _animator;
    
    public Action Interacted;
    public Action<bool> Approached;
    public static Action<string[]> DialogueStarted;

    private bool _interactable;
    public bool Interactable => _interactable;

    protected bool _talking;
    
    private int IsTalking = Animator.StringToHash("isTalking");

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

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    protected virtual void Talk()
    {
        if (!_talking)
        {
            DialogueStarted?.Invoke(_lines);
            _talking = true;
            if (_animator) _animator.SetTrigger(IsTalking);
        }
    }

    protected virtual void OnApproach(bool interactable)
    {
        _interactable = interactable;
    }

    protected virtual void StopTalking()
    {
        _talking = false;
    }
}
