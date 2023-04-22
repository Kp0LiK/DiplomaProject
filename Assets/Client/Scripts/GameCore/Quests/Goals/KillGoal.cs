using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillGoal : Quest.QuestGoal
{
    [SerializeField] private string _target;

    public string Target => _target;
    
    public override string GetDescription()
    {
        return "Kill " + _target + "!";
    }

    public override void Initialize()
    {
        base.Initialize();
        _name = "Kill";
        EventManager.Instance.AddListener<KillingGameEvent>(OnKill);
    }

    private void OnKill(KillingGameEvent eventInfo)
    {
        if (eventInfo.TargetName == _target)
        {
            CurrentAmount++;
            Evaluate();
        }
    }
}
