using System;
using UnityEngine;

public class GameSession : MonoBehaviour
{
    private int _score;

    public int Score
    {
        get => _score;
        set
        {
            PlayerPrefs.SetInt("Score", value);
            ScoreChanged?.Invoke(value);
            _score = value;
            PlayerPrefs.Save();
        }
    }

    public event Action <int> ScoreChanged;

    private void Start()
    {
        _score = PlayerPrefs.HasKey("Score") ? PlayerPrefs.GetInt("Score") : 100;
    }
}
