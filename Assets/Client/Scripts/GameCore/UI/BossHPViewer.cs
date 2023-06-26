using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class BossHPViewer : MonoBehaviour
{
    private Text _name;
    private Slider _HpSlider;
    private CanvasGroup _canvasGroup;

    public static Action<string> OnBossEnter;
    public static Action<float> OnHealthInitialized;
    public static Action<float> OnHealthChanged;
    public static Action OnBossDeath;

    private void OnEnable()
    {
        OnBossEnter += ChangeName;
        OnBossDeath += Disappear;
        OnHealthChanged += ChangeHealth;
        OnHealthInitialized += InitializeHealth;
    }

    private void OnDisable()
    {
        OnBossEnter -= ChangeName;
        OnBossDeath -= Disappear;
        OnHealthChanged -= ChangeHealth;
        OnHealthInitialized -= InitializeHealth;
    }

    private void Awake()
    {
        _name = GetComponentInChildren<Text>();
        _HpSlider = GetComponentInChildren<Slider>();
        _canvasGroup = GetComponent<CanvasGroup>();
        _canvasGroup.alpha = 0;
    }

    private void Appear()
    {
        _canvasGroup.alpha = 1;
    }

    private void Disappear()
    {
        _canvasGroup.alpha = 0;
    }

    private void ChangeName(string name)
    {
        _name.text = name;
        Appear();
    }

    private void ChangeHealth(float health)
    {
        _HpSlider.DOValue(health, 0.5f);
    }

    private void InitializeHealth(float health)
    {
        _HpSlider.maxValue = health;
        _HpSlider.value = health;
    }
}
