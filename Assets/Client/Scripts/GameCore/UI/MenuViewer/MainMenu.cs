using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Zenject;

namespace Client
{
    [RequireComponent(typeof(AudioSource))]
    public class MainMenu : BaseWindowManager, IAudioPlayable
    {
        [field: SerializeField] public AudioSource Source { get; private set; }
        [field: SerializeField] public List<AudioData> AudioData { get; private set; }

        [field: SerializeField] public AudioMixerGroup SoundMixer {get; private set;}
        [field: SerializeField] public AudioMixerGroup MusicMixer {get; private set;}
        
        public GameSession GameSession { get; private set; }
    
    

        [Inject]
        public void Constructor(GameSession gameSession)
        {
            GameSession = gameSession;
        }

        protected override void Start()
        {
            base.Start();
        
            this.PlayOneShoot(AudioData.GetData(AudioType.MainMenuMusic));
        }
    }
}

