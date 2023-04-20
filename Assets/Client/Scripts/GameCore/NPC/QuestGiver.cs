using System;
using UnityEngine;

namespace Client
{
    public class QuestGiver : NPC
    {
        [SerializeField] private Quest _quest;

        public static Action<Quest> OnQuestGiven;
        
        protected override void Talk()
        {
            base.Talk();
            GiveQuest();
        }

        private void GiveQuest()
        {
            OnQuestGiven?.Invoke(_quest);
        }
    }
}