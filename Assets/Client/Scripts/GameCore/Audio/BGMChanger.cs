using System;
using Client;
using UnityEngine;

public class BGMChanger : MonoBehaviour
{
    private AudioSource _audioSource;
    [SerializeField] private AudioClip _audioClip;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerBehaviour _))
        {
            _audioSource.clip = _audioClip;
            _audioSource.Play();
        }
    }
}
