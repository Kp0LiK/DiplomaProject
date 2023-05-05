using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialSystem : MonoBehaviour
{
    [SerializeField] private string[] _greetingLines;
    
    [SerializeField] private string[] _npcInteractionLines;
    [SerializeField] private string[] _postNpcInteractionLines;
    
    [SerializeField] private string[] _questGiverInteractionLines;
    [SerializeField] private string[] _postQuestGiverInteractionLines;

    [SerializeField] private string[] _itemCollectedLines;
    [SerializeField] private string[] _questCompleteLines;

    private bool _hasGreeted;
    
    private bool _hasNPCInteracted;
    private bool _hasPostNPCInteracted;

    private bool _hasQuestGiverInteracted;
    private bool _hasPostQuestGiverInteracted;

    private bool _hasCollectedItem;
    private bool _hasCompletedQuest;
    
    public static Action<string[]> DialogueStarted;
    
    public bool IsExplaining;

    private void OnEnable()
    {
        TutorialNPC.OnInteractable += StartNPCInteractionTutorial;
        TutorialQuestGiver.OnInteractable += StartQuestGiverTutorial;
        DialogueSystem.DialogueEnded += StopExplaining;
    }

    private void OnDisable()
    {
        TutorialNPC.OnInteractable -= StartNPCInteractionTutorial;
        TutorialQuestGiver.OnInteractable -= StartQuestGiverTutorial;
        DialogueSystem.DialogueEnded -= StopExplaining;
    }

    private void Start()
    {
        if (!IsExplaining)
        {
            StartCoroutine(BeginTutorial(2f, _greetingLines));
        }
    }

    private void StartNPCInteractionTutorial()
    {
        if (_hasNPCInteracted) return;
        
        StartCoroutine(BeginTutorial(1f, _npcInteractionLines));
        _hasNPCInteracted = true;
    }

    public void AfterNPCInteraction()
    {
        if (_hasPostNPCInteracted) return;
        
        StartCoroutine(BeginTutorial(1f, _postNpcInteractionLines));
        _hasPostNPCInteracted = true;
    }

    private void StartQuestGiverTutorial()
    {
        if (_hasQuestGiverInteracted) return;

        StartCoroutine(BeginTutorial(1f, _questGiverInteractionLines));
        _hasQuestGiverInteracted = true;
    }

    public void AfterQuestGiverInteraction()
    {
        if (_hasPostQuestGiverInteracted) return;
        
        StartCoroutine(BeginTutorial(1f, _postQuestGiverInteractionLines));
        _hasPostQuestGiverInteracted = true;
    }

    public void AfterItemCollected()
    {
        if (_hasCollectedItem) return;

        StartCoroutine(BeginTutorial(1f, _itemCollectedLines));
        _hasCollectedItem = true;
    }

    public void AfterQuestCompleted()
    {
        if (_hasCompletedQuest) return;

        StartCoroutine(BeginTutorial(1f, _questCompleteLines));
        _hasCompletedQuest = true;
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