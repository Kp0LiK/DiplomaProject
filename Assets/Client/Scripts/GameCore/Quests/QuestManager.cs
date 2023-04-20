using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
    [SerializeField] private GameObject _questPrefab;
    [SerializeField] private Transform _questsContent;
    [SerializeField] private GameObject _questHolder;

    public List<Quest> CurrentQuests;

    private void OnEnable()
    {
        Collectible.OnCollected += Collect;
        Target.OnKilled += Kill;
        Talker.OnTalked += Talk;
        Destination.OnReached += Reach;
    }

    private void OnDisable()
    {
        Collectible.OnCollected -= Collect;
        Target.OnKilled -= Kill;
        Talker.OnTalked -= Talk;
        Destination.OnReached -= Reach;
    }

    private void Awake()
    {
        Debug.Log("hello");
        Initialize();
    }

    public void Initialize()
    {
        foreach (var quest in CurrentQuests)
        {
            quest.Initialize();
            quest.QuestCompleted.AddListener(OnQuestCompleted);

            GameObject questObj = Instantiate(_questPrefab, _questsContent);
            questObj.transform.Find("Icon").GetComponent<Image>().sprite = quest.Information.Icon;
            
            questObj.GetComponent<Button>().onClick.AddListener(delegate
            {
                _questHolder.GetComponent<QuestWindow>().Initialize(quest);
                _questHolder.SetActive(true);
            });
            
            quest.Evaluate();
        }
    }

    public void AddQuest(Quest quest)
    {
        CurrentQuests.Add(quest);
        quest.Initialize();
        quest.QuestCompleted.AddListener(OnQuestCompleted);

        GameObject questObj = Instantiate(_questPrefab, _questsContent);
        questObj.transform.Find("Icon").GetComponent<Image>().sprite = quest.Information.Icon;
            
        questObj.GetComponent<Button>().onClick.AddListener(delegate
        {
            _questHolder.GetComponent<QuestWindow>().Initialize(quest);
            _questHolder.SetActive(true);
        });
        
        quest.Evaluate();
    }

    public void Collect(string collectibleName)
    {
        List<Quest.QuestGoal> collectGoals = GetCollectGoals();

        foreach (CollectGoal collectGoal in collectGoals)
        {
            if (collectGoal.Collectible == collectibleName) EventManager.Instance.QueueEvent(new CollectingGameEvent(collectibleName));
        }
    }

    public void Kill(string targetName)
    {
        List<Quest.QuestGoal> killGoals = GetKillGoals();

        foreach (KillGoal killGoal in killGoals)
        {
            if (killGoal.Target == targetName) EventManager.Instance.QueueEvent(new KillingGameEvent(targetName));
        }
    }

    public void Talk(string talkerName)
    {
        List<Quest.QuestGoal> talkGoals = GetTalkGoals();

        foreach (TalkGoal talkGoal in talkGoals)
        {
            if (talkGoal.Talker == talkerName) EventManager.Instance.QueueEvent(new TalkingGameEvent(talkerName)); 
        }
    }

    public void Reach(string destinationName)
    {
        List<Quest.QuestGoal> reachGoals = GetReachGoals();

        foreach (ReachGoal reachGoal in reachGoals)
        {
            if (reachGoal.Destination == destinationName) EventManager.Instance.QueueEvent(new ReachingGameEvent(destinationName));
        }
    }
    
    private List<Quest.QuestGoal> GetCollectGoals()
    {
        List<Quest.QuestGoal> collectGoals = new List<Quest.QuestGoal>();
        
        foreach (var quest in CurrentQuests)
        {
            collectGoals.AddRange(quest.Goals.FindAll(goal => goal.GetName() == "Collect"));
        }

        return collectGoals;
    }

    private List<Quest.QuestGoal> GetKillGoals()
    {
        List<Quest.QuestGoal> killGoals = new List<Quest.QuestGoal>();
        
        foreach (var quest in CurrentQuests)
        {
            killGoals.AddRange(quest.Goals.FindAll(goal => goal.GetName() == "Kill"));
        }

        return killGoals;
    }
    
    private List<Quest.QuestGoal> GetTalkGoals()
    {
        List<Quest.QuestGoal> talkGoals = new List<Quest.QuestGoal>();
        
        foreach (var quest in CurrentQuests)
        {
            talkGoals.AddRange(quest.Goals.FindAll(goal => goal.GetName() == "Talk"));
        }

        return talkGoals;
    }
    
    private List<Quest.QuestGoal> GetReachGoals()
    {
        List<Quest.QuestGoal> reachGoals = new List<Quest.QuestGoal>();
        
        foreach (var quest in CurrentQuests)
        {
            reachGoals.AddRange(quest.Goals.FindAll(goal => goal.GetName() == "Reach"));
        }

        return reachGoals;
    }

    private void OnQuestCompleted(Quest quest)
    {
        // Destroy(_questsContent.GetChild(CurrentQuests.IndexOf(quest)).gameObject);
        _questsContent.GetChild(CurrentQuests.IndexOf(quest)).Find("Checkmark").gameObject.SetActive(true);
    }
}
