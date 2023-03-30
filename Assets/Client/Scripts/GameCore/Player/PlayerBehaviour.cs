using System;
using System.Collections;
using System.Collections.Generic;
using Client.Scripts.Data.Player;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField] private PlayerData _playerData;
    private NPC _currentNPC;
    private Quest _currentQuest;

    private Animator _animator;
    public event Action<int> HealthChanged;


    private void Awake()
    {
        _animator = GetComponent<Animator>();
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

        if (Input.GetKeyDown(KeyCode.F) && _currentQuest != null)
        {
            if (_currentQuest.IsActive)
            {
                _currentQuest.QuestGoal.EnemyKilled();
                if (_currentQuest.QuestGoal.IsReached())
                {
                    _currentQuest.CompleteQuest();
                }
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

    public void SetActiveQuest(Quest quest)
    {
        _currentQuest = quest;
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
