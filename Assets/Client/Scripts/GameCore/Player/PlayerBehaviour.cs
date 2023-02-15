using System;
using System.Collections;
using System.Collections.Generic;
using Client.Scripts.Data.Player;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField] private PlayerData _playerData;

    private Animator _animator;
    public event Action<int> HealthChanged;


    private void Awake()
    {
        _animator = GetComponent<Animator>();
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
