using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialSystem : MonoBehaviour
{
    [SerializeField] private string[] _greetingLines;
    [SerializeField] private string[] _interactionLines;
    [SerializeField] private string[] _postInteractionLines;

    private bool _hasGreeted;
    private bool _hasInteracted;
    private bool _hasPostInteracted;
    
    public static Action<string[]> DialogueStarted;
    
    public bool IsExplaining;

    private void OnEnable()
    {
        TutorialNPC.OnInteractable += StartInteractionTutorial;
        DialogueSystem.DialogueEnded += StopExplaining;
    }

    private void OnDisable()
    {
        TutorialNPC.OnInteractable -= StartInteractionTutorial;
        DialogueSystem.DialogueEnded -= StopExplaining;
    }

    private void Start()
    {
        if (!IsExplaining)
        {
            StartCoroutine(BeginTutorial(2f, _greetingLines));
        }
    }

    private void StartInteractionTutorial()
    {
        if (_hasInteracted) return;
        
        StartCoroutine(BeginTutorial(1f, _interactionLines));
        _hasInteracted = true;
    }

    public void AfterInteraction()
    {
        if (_hasPostInteracted) return;
        
        StartCoroutine(BeginTutorial(1f, _postInteractionLines));
        _hasPostInteracted = true;
    }

    // Этот метод можно переиспользовать, чтобы включить туториал при надобности
    private IEnumerator BeginTutorial(float seconds, string[] lines)
    {
        yield return new WaitForSeconds(seconds);
        DialogueStarted?.Invoke(lines);
        IsExplaining = true;
    }

    private void StopExplaining()
    {
        IsExplaining = false;
    }
}