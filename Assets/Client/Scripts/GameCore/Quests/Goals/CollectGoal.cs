using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectGoal : Quest.QuestGoal
{
    [SerializeField] private string _collectible;

    public string Collectible => _collectible;

    public override string GetDescription()
    {
        return "Collect " + _collectible + "!";
    }

    public override void Initialize()
    {
        base.Initialize();
        _name = "Collect";
        EventManager.Instance.AddListener<CollectingGameEvent>(OnCollect);
    }

    private void OnCollect(CollectingGameEvent eventInfo)
    {
        if (eventInfo.CollectibleName == _collectible)
        {
            CurrentAmount++;
            Evaluate();
        }
    }
}
