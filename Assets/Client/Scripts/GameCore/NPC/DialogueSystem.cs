using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour
{
    [SerializeField] private Text _text;
    [SerializeField] private float _textSpeed;
    private string[] _lines;

    private int _index;
    private Image _image;

    public static Action DialogueEnded;

    private void OnEnable()
    {
        NPC.DialogueStarted += StartDialogue;
        TutorialSystem.DialogueStarted += StartDialogue;
    }

    private void OnDisable()
    {
        NPC.DialogueStarted -= StartDialogue;
        TutorialSystem.DialogueStarted -= StartDialogue;
    }

    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (_lines != null)
            {
                if (_text.text == _lines[_index])
                {
                    NextLine();
                }
                else
                {
                    StopAllCoroutines();
                    _text.text = _lines[_index];
                }
            }
        }
    }

    private void StartDialogue(string[] lines)
    {
        SetActive(true);
        _text.text = string.Empty;
        _lines = lines;
        _index = 0;
        StartCoroutine(TypeLine());
    }

    private IEnumerator TypeLine()
    {
        foreach (char c in _lines[_index])
        {
            _text.text += c;
            yield return new WaitForSeconds(_textSpeed);
        }
    }

    private void NextLine()
    {
        if (_index < _lines.Length - 1)
        {
            _index++;
            _text.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            SetActive(false);
            DialogueEnded?.Invoke();
        }
    }

    private void SetActive(bool active)
    {
        var tempColor = _image.color;

        tempColor.a = active ? 1f : 0f;
        
        _image.color = tempColor;
        
        _text.gameObject.SetActive(active);
    }
}
