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

    private void Awake()
    {
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
        }
    }

    public void AddQuest(Quest quest)
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
    }

    public void Collect(string collectibleName)
    {
        EventManager.Instance.QueueEvent(new CollectingGameEvent(collectibleName));
    }

    private void OnQuestCompleted(Quest quest)
    {
        _questsContent.GetChild(CurrentQuests.IndexOf(quest)).Find("Checkmark").gameObject.SetActive(true);
    }
}
