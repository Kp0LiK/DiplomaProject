using System;
using System.Collections;
using System.Collections.Generic;
using Client;
using UnityEngine;

public class AudioCue : MonoBehaviour
{
    [SerializeField] private string _name;
    private bool _wasPlayed;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerBehaviour _) && !_wasPlayed)
        {
            switch (_name)
            {
                case "Spiders": PlayerBehaviour.OnManySpiders?.Invoke();
                    break;
                case "Goblins": PlayerBehaviour.OnGoblins?.Invoke();
                    break;
                case "Confusing": PlayerBehaviour.OnConfusingRoad?.Invoke();
                    break;
                case "Dragon": PlayerBehaviour.OnBigDragon?.Invoke();
                    break;
                case "Guardian": PlayerBehaviour.OnGuardian?.Invoke();
                    break;
            }

            _wasPlayed = true;
        }
    }
}
