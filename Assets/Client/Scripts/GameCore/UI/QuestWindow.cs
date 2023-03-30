using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestWindow : MonoBehaviour
{
    [SerializeField] private Text _title;
    [SerializeField] private Text _description;
    [SerializeField] private Text _experience;
    [SerializeField] private Text _gold;
    [SerializeField] private GameObject _panel;
    [SerializeField] private Button _acceptButton;

    public static Action<string, string, int, int> Activated;
    public static Action QuestAccepted;
    public static Action<string, string> QuestCompleted;

    private void OnEnable()
    {
        Activated += OpenQuestWindow;
        QuestCompleted += CompleteQuest;
    }

    private void OnDisable()
    {
        Activated -= OpenQuestWindow;
        QuestCompleted -= CompleteQuest;
    }

    private void OpenQuestWindow(string title, string description, int exp, int gold)
    {
        _title.text = title;
        _description.text = description;
        _experience.text = exp.ToString() + " EXP";
        _gold.text = gold.ToString() + " gold";
        
        _panel.SetActive(true);
    }

    private void CompleteQuest(string title, string description)
    {
        _title.text = title;
        _description.text = description;
        _acceptButton.gameObject.SetActive(false);
        _panel.SetActive(true);
    }

    public void OnExit()
    {
        _panel.SetActive(false);
    }

    public void OnAccept()
    {
        _panel.SetActive(false);
        QuestAccepted?.Invoke();
    }
}
