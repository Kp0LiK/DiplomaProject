using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest
{
    [SerializeField] private bool _isActive;
    public bool IsActive => _isActive;

    [SerializeField] private string _title;
    public string Title => _title;
    [SerializeField] private string _description;
    public string Description => _description;
    [SerializeField] private int _experienceReward;
    public int ExperienceReward => _experienceReward;
    [SerializeField] private int _goldReward;
    public int GoldReward => _goldReward;

    [SerializeField] private QuestGoal _questGoal;
    public QuestGoal QuestGoal => _questGoal;

    public void CompleteQuest()
    {
        Debug.Log("Quest is complete!");
        DeactivateQuest();
        QuestWindow.QuestCompleted?.Invoke("Thanks", "Here is your reward");
    }

    public void ActivateQuest()
    {
        _isActive = true;
    }

    public void DeactivateQuest()
    {
        _isActive = false;
    }
}
