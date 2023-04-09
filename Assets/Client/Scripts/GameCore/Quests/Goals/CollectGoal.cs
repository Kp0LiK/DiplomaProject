using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectGoal : Quest.QuestGoal
{
    [SerializeField] private string Collectible;
    
    public override string GetDescription()
    {
        return "Collect " + Collectible + " !";
    }

    public override void Initialize()
    {
        base.Initialize();
        EventManager.Instance.AddListener<CollectingGameEvent>(OnCollect);
    }

    private void OnCollect(CollectingGameEvent eventInfo)
    {
        if (eventInfo.CollectibleName == Collectible)
        {
            CurrentAmount++;
            Evaluate();
        }
    }
}
