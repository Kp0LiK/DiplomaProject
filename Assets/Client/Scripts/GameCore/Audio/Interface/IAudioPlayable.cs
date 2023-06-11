using System.Collections.Generic;
using UnityEngine;

public interface IAudioPlayable
{
    public AudioSource Source { get;}
    List<AudioData> AudioData { get; }
}