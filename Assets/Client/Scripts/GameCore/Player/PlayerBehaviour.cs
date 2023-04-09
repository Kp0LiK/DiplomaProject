using System;
using System.Collections;
using System.Collections.Generic;
using Client.Scripts.Data.Player;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    /// Attach a quest to NPC done!
    /// After talking to the NPC, add quest to the list (maybe with Action smth like OnConvoFinished) done!
    /// Pray it appears done!
    /// Complete it
    [SerializeField] private PlayerData _playerData;
    [SerializeField] private QuestManager _questManager;
    private NPC _currentNPC;

    private Animator _animator;
    public event Action<int> HealthChanged;


    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        QuestGiver.OnQuestGiven += AddQuest;
    }

    private void OnDisable()
    {
        QuestGiver.OnQuestGiven -= AddQuest;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && _currentNPC)
        {
            if (_currentNPC.Interactable)
            {
                _currentNPC.Interacted?.Invoke();
            }
        }
    }

    public void ApplyDamage(float damage)
    {
        if (_playerData.Health <= 0)
        {
            _playerData.Health = 0;
            _playerData.IsDied = true;
        }

        if (_playerData.IsDied)
        {
            //todo death state
        }
        else
        {
            _playerData.Health -= damage;
        }

        UpdateHealth();
    }

    private void UpdateHealth()
    {
        HealthChanged?.Invoke(Mathf.RoundToInt(_playerData.Health));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out NPC npc))
        {
            npc.Approached?.Invoke(true);
            _currentNPC = npc;
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out NPC npc))
        {
            npc.Approached?.Invoke(false);
            _currentNPC = null;
        }
    }

    private void AddQuest(Quest quest)
    {
        if (_questManager.CurrentQuests.Contains(quest)) return;
        
        _questManager.CurrentQuests.Add(quest);
        _questManager.AddQuest(quest);
    }

    [Serializable]
    public class PlayerAudioData
    {
        public AudioClip OnAttack;
        public AudioClip OnDetect;
        public AudioClip OnUnDetect;
        public AudioClip OnMove;
        public AudioClip OnHeal;
        public AudioClip OnCast;
        public AudioClip OnDie;
    }
}
