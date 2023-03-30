using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestGoal
{
    [SerializeField] private GoalType _goalType;

    [SerializeField] private int _requiredAmount;
    [SerializeField] private int _currentAmount;

    public bool IsReached()
    {
        return (_currentAmount >= _requiredAmount);
    }

    public void EnemyKilled()
    {
        if (_goalType == GoalType.Kill)
        {
            _currentAmount++;
        }
    }
}

public enum GoalType
{
    Kill,
    Gather
}