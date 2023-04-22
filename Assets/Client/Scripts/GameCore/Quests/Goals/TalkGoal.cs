using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkGoal : Quest.QuestGoal
{
    [SerializeField] private string _talker;

    public string Talker => _talker;
    
    public override string GetDescription()
    {
        return "Talk to " + _talker + "!";
    }
    
    public override void Initialize()
    {
        base.Initialize();
        _name = "Talk";
        EventManager.Instance.AddListener<TalkingGameEvent>(OnTalk);
    }

    private void OnTalk(TalkingGameEvent eventInfo)
    {
        if (eventInfo.TalkerName == _talker)
        {
            CurrentAmount++;
            Evaluate();
        }
    }
}
