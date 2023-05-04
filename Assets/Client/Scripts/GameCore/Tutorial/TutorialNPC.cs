using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialNPC : NPC
{
    [SerializeField] private TutorialSystem _tutorialSystem;

    public static Action OnInteractable;

    private bool _stoppedTalking;

    protected override void Talk()
    {
        if (!_talking && !_tutorialSystem.IsExplaining && !_stoppedTalking)
        {
            DialogueStarted?.Invoke(_lines);
            _talking = true;
            _stoppedTalking = true;
        }
    }
    
    protected override void OnApproach(bool interactable)
    {
        base.OnApproach(interactable);
        OnInteractable?.Invoke();
    }

    protected override void StopTalking()
    {
        base.StopTalking();
        if (_stoppedTalking) _tutorialSystem.AfterInteraction();
    }
}
