using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReachGoal :Quest.QuestGoal
{
    [SerializeField] private string _destination;

    public string Destination => _destination;
    
    public override string GetDescription()
    {
        return "Reach " + _destination + "!";
    }
    
    public override void Initialize()
    {
        base.Initialize();
        _name = "Reach";
        EventManager.Instance.AddListener<ReachingGameEvent>(OnReach);
    }

    private void OnReach(ReachingGameEvent eventInfo)
    {
        if (eventInfo.DestinationName == _destination)
        {
            CurrentAmount++;
            Evaluate();
        }
    }
}
