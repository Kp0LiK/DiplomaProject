using System;
using System.Collections;
using System.Collections.Generic;
using Client;
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

    [SerializeField] private string[] _weaponLines;
    [SerializeField] private string[] _swordLines;
    [SerializeField] private string[] _bowLines;
    [SerializeField] private string[] _magicLines;

    [SerializeField] private string[] _enemyLines;
    [SerializeField] private string[] _afterKillingEnemyLines;

    private bool _hasGreeted;
    
    private bool _hasNPCInteracted;
    private bool _hasPostNPCInteracted;

    private bool _hasQuestGiverInteracted;
    private bool _hasPostQuestGiverInteracted;

    private bool _hasCollectedItem;
    private bool _hasCompletedQuest;

    private bool _hasWeapon;
    private bool _hasSword;
    private bool _hasBow;
    private bool _hasMagic;

    private bool _hasEnemy;
    private bool _killedEnemy;
    
    public static Action<string[]> DialogueStarted;
    
    private int index;
    
    public bool IsExplaining;

    private void OnEnable()
    {
        TutorialNPC.OnInteractable += StartNPCInteractionTutorial;
        TutorialQuestGiver.OnInteractable += StartQuestGiverTutorial;
        DialogueSystem.DialogueEnded += StopExplaining;
        SpiderBehaviour.OnDeath += AfterEnemyKilled;
    }

    private void OnDisable()
    {
        TutorialNPC.OnInteractable -= StartNPCInteractionTutorial;
        TutorialQuestGiver.OnInteractable -= StartQuestGiverTutorial;
        DialogueSystem.DialogueEnded -= StopExplaining;
        SpiderBehaviour.OnDeath -= AfterEnemyKilled;
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

        StartCoroutine(BeginTutorial(0.5f, _itemCollectedLines));
        _hasCollectedItem = true;
    }

    public void AfterQuestCompleted()
    {
        if (_hasCompletedQuest) return;

        StartCoroutine(BeginTutorial(1f, _questCompleteLines));
        _hasCompletedQuest = true;

        index = 1;
    }

    public void ExplainingWeapon()
    {
        if (_hasWeapon) return;

        StartCoroutine(BeginTutorial(1f, _weaponLines));
        _hasWeapon = true;

        index = 2;
    }

    public void ExplainingSword()
    {
        if (_hasSword) return;

        StartCoroutine(BeginTutorial(1f, _swordLines));
        _hasSword = true;

        index = 3;
    }

    public void ExplainingBow()
    {
        if (_hasBow) return;

        StartCoroutine(BeginTutorial(1f, _bowLines));
        _hasBow = true;

        index = 4;
    }

    public void ExplainingMagic()
    {
        if (_hasMagic) return;
        
        StartCoroutine(BeginTutorial(1f, _magicLines));
        _hasMagic = true;

        index = 5;
    }

    public void ExplainingEnemy()
    {
        if (_hasEnemy) return;

        StartCoroutine(BeginTutorial(1f, _enemyLines));
        _hasEnemy = true;
    }

    public void AfterEnemyKilled()
    {
        if (_killedEnemy) return;

        StartCoroutine(BeginTutorial(1f, _afterKillingEnemyLines));
        _killedEnemy = true;
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

        switch (index)
        {
            case 1: ExplainingWeapon();
                break;
            case 2: ExplainingSword();
                break;
            case 3: ExplainingBow();
                break;
            case 4: ExplainingMagic();
                break;
            case 5: ExplainingEnemy();
                break;
        }
    }
}