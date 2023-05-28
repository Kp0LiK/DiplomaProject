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
    public static Action<bool> SetPlayerMovement;
    public static Action<bool> SetPlayerAttack;
    public static Action<bool> SetPlayerInteraction;
    
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
            SetPlayerMovement?.Invoke(false);
            SetPlayerAttack?.Invoke(false);
            SetPlayerInteraction?.Invoke(false);
            index = 1;
        }
    }

    private void StartNPCInteractionTutorial()
    {
        if (_hasNPCInteracted) return;
        
        StartCoroutine(BeginTutorial(1f, _npcInteractionLines));
        SetPlayerMovement?.Invoke(false);
        _hasNPCInteracted = true;
        index = 2;
    }

    public void AfterNPCInteraction()
    {
        if (_hasPostNPCInteracted) return;
        
        StartCoroutine(BeginTutorial(1f, _postNpcInteractionLines));
        Debug.Log("Post NPC");
        SetPlayerMovement?.Invoke(false);
        _hasPostNPCInteracted = true;
        index = 3;
    }

    private void StartQuestGiverTutorial()
    {
        if (_hasQuestGiverInteracted) return;

        StartCoroutine(BeginTutorial(1f, _questGiverInteractionLines));
        SetPlayerMovement?.Invoke(false);
        _hasQuestGiverInteracted = true;
        index = 4;
    }

    public void AfterQuestGiverInteraction()
    {
        if (_hasPostQuestGiverInteracted) return;
        
        StartCoroutine(BeginTutorial(1f, _postQuestGiverInteractionLines));
        SetPlayerMovement?.Invoke(false);
        _hasPostQuestGiverInteracted = true;
        index = 5;
    }

    public void AfterItemCollected()
    {
        if (_hasCollectedItem) return;

        StartCoroutine(BeginTutorial(0.5f, _itemCollectedLines));
        SetPlayerMovement?.Invoke(false);
        _hasCollectedItem = true;
        index = 6;
    }

    public void AfterQuestCompleted()
    {
        if (_hasCompletedQuest) return;

        StartCoroutine(BeginTutorial(1f, _questCompleteLines));
        SetPlayerMovement?.Invoke(false);
        _hasCompletedQuest = true;

        index = 7;
    }

    public void ExplainingWeapon()
    {
        if (_hasWeapon) return;

        StartCoroutine(BeginTutorial(1f, _weaponLines));
        _hasWeapon = true;

        index = 8;
    }

    public void ExplainingSword()
    {
        if (_hasSword) return;

        StartCoroutine(BeginTutorial(1f, _swordLines));
        _hasSword = true;

        index = 9;
    }

    public void ExplainingBow()
    {
        if (_hasBow) return;

        StartCoroutine(BeginTutorial(1f, _bowLines));
        _hasBow = true;

        index = 10;
    }

    public void ExplainingMagic()
    {
        if (_hasMagic) return;
        
        StartCoroutine(BeginTutorial(1f, _magicLines));
        _hasMagic = true;

        index = 11;
    }

    public void ExplainingEnemy()
    {
        if (_hasEnemy) return;

        StartCoroutine(BeginTutorial(1f, _enemyLines));
        _hasEnemy = true;
        index = 12;
    }

    public void AfterEnemyKilled()
    {
        if (_killedEnemy) return;

        StartCoroutine(BeginTutorial(1f, _afterKillingEnemyLines));
        SetPlayerMovement?.Invoke(false);
        SetPlayerAttack?.Invoke(false);
        _killedEnemy = true;
        index = 13;
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
            case 1:
            case 3:
            case 5:
            case 6: SetPlayerMovement?.Invoke(true);
                break;
            case 2:
            case 4: SetPlayerInteraction?.Invoke(true);
                break;
            case 7: ExplainingWeapon();
                break;
            case 8: ExplainingSword();
                break;
            case 9: ExplainingBow();
                break;
            case 10: ExplainingMagic();
                break;
            case 11: ExplainingEnemy();
                break;
            case 12: SetPlayerMovement?.Invoke(true); SetPlayerAttack?.Invoke(true);
                break;
        }
    }
}