using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestWindow : MonoBehaviour
{
    [SerializeField] private Text _titleText;
    [SerializeField] private Text _descriptionText;
    [SerializeField] private GameObject _goalPrefab;
    [SerializeField] private Transform _goalsContent;
    [SerializeField] private Text _xpText;
    private bool _isInitialized;

    public void Initialize(Quest quest)
    {
        if (!_isInitialized)
        {
            _titleText.text = quest.Information.Name;
            _descriptionText.text = quest.Information.Description;

            foreach (var goal in quest.Goals)
            {
                GameObject goalObj = Instantiate(_goalPrefab, _goalsContent);
                goalObj.transform.Find("Text").GetComponent<Text>().text = goal.GetDescription();

                GameObject countObj = goalObj.transform.Find("Count").gameObject;

                if (goal.Completed)
                {
                    countObj.SetActive(false);
                    goalObj.transform.Find("Done").gameObject.SetActive(true);
                }
                else
                {
                    countObj.GetComponent<Text>().text = goal.CurrentAmount + "/" + goal.RequiredAmount;
                }
            }

            _xpText.text = quest.Reward.XP.ToString();
            _isInitialized = true;
        }
    }

    public void CloseWindow()
    {
        _isInitialized = false;
        gameObject.SetActive(false);

        for (int i = 0; i < _goalsContent.childCount; i++)
        {
            Destroy(_goalsContent.GetChild(i).gameObject);
        }
    }
}
