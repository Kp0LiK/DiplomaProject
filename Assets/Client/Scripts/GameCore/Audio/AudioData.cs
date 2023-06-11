using System;
using UnityEngine;

[Serializable]
public class AudioData
{
    [field: SerializeField] public AudioClip Clip { get; private set; }
    [field: SerializeField] public AudioType Type { get; private set; }

    public AudioData(AudioClip clip, AudioType type)
    {
        Clip = clip;
        Type = type;
    }
}
