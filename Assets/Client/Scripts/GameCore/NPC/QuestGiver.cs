using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGiver : NPC
{
    [SerializeField] private Quest quest;

    public static Action<Quest> OnQuestGiven;
    
    protected override void OnDisable()
    {
        base.OnDisable();
        DialogueSystem.DialogueEnded -= GiveQuest;
    }

    protected override void Talk()
    {
        base.Talk();
        DialogueSystem.DialogueEnded += GiveQuest;
    }

    protected void GiveQuest()
    {
        OnQuestGiven?.Invoke(quest);
    }
}
