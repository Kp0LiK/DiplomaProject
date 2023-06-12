using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Client
{
    public class TutorialQuestManager : QuestManager
    {
        [SerializeField] private TutorialSystem _tutorialSystem;

        protected override void Collect(string collectibleName, GameObject collectibleObject)
        {
            List<Quest.QuestGoal> collectGoals = GetCollectGoals();

            foreach (CollectGoal collectGoal in collectGoals)
            {
                if (collectGoal.Collectible == collectibleName)
                {
                    EventManager.Instance.QueueEvent(new CollectingGameEvent(collectibleName));
                    Destroy(collectibleObject);
                    _tutorialSystem.AfterItemCollected();
                }
            }
        }

        protected override void OnQuestCompleted(Quest quest)
        {
            base.OnQuestCompleted(quest);
            _tutorialSystem.AfterQuestCompleted();
        }
    }
}

