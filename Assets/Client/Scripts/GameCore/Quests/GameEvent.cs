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
