using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameEvent
{
    public string EventDescription;
}

public class CollectingGameEvent : GameEvent
{
    public string CollectibleName;

    public CollectingGameEvent(string name)
    {
        CollectibleName = name;
    }
}

public class KillingGameEvent : GameEvent
{
    public string TargetName;

    public KillingGameEvent(string name)
    {
        TargetName = name;
    }
}

public class TalkingGameEvent : GameEvent
{
    public string TalkerName;

    public TalkingGameEvent(string name)
    {
        TalkerName = name;
    }
}

public class ReachingGameEvent : GameEvent
{
    public string DestinationName;

    public ReachingGameEvent(string name)
    {
        DestinationName = name;
    }
}