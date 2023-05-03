using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialSystem : MonoBehaviour
{
    [SerializeField] private string[] _greetingLines;
    
    public static Action<string[]> DialogueStarted;
    private bool _talking;

    private void Start()
    {
        if (!_talking)
        {
            StartCoroutine(BeginTutorial(2f, _greetingLines));
        }
    }

    // Этот метод можно переиспользовать, чтобы включить туториал при надобности
    private IEnumerator BeginTutorial(float seconds, string[] lines)
    {
        yield return new WaitForSeconds(seconds);
        DialogueStarted?.Invoke(lines);
        _talking = true;
    }
}