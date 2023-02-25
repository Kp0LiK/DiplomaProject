using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGiver : NPC
{
    [SerializeField] private Quest _quest;
    [SerializeField] private PlayerBehaviour _playerBehaviour;
    
    public Quest Quest => _quest;

    protected override void Talk()
    {
        base.Talk();
        GiveQuest();
    }

    private void GiveQuest()
    {
        QuestWindow.Activated?.Invoke(_quest.Title, _quest.Description, _quest.ExperienceReward, _quest.GoldReward);
        QuestWindow.QuestAccepted += AssignQuest;
    }

    private void AssignQuest()
    {
        _quest.ActivateQuest();
        _playerBehaviour.SetActiveQuest(_quest);
        QuestWindow.QuestAccepted -= AssignQuest;
    }

}
