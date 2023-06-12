using System;
using System.Collections;
using System.Collections.Generic;
using Client;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class MoneyViewer : MonoBehaviour
{
    [SerializeField] private Text _moneyText;
    
    private int _money;
    private GameSession _gameSession;
    
    [Inject]
    public void Constructor(GameSession gameSession)
    {
        _gameSession = gameSession;
    }

    private void OnEnable()
    {
        _gameSession.MoneyChanged += UpdateMoney;
    }

    private void OnDisable()
    {
        _gameSession.MoneyChanged -= UpdateMoney;
    }

    private void Awake()
    {
        _money = _gameSession.Money;
        _moneyText.text = _money.ToString();
    }

    private void UpdateMoney(int money)
    {
        _money = money;
        _moneyText.text = _money.ToString();
    }
}
