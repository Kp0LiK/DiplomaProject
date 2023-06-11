using UnityEngine;
using UnityEngine.Audio;

namespace Client
{
    public class PlayerSetting
    {
        public float MusicVolume
        {
            get
            {
                _audioMixer.SetFloat("MusicVolume", PlayerPrefs.GetFloat("MusicVolume"));
                return PlayerPrefs.GetFloat("MusicVolume");
            }
            set
            {
                PlayerPrefs.SetFloat("MusicVolume", value);

                _audioMixer.SetFloat("MusicVolume", value);
                _musicVolume = value;

                PlayerPrefs.Save();
            }
        }

        public float EffectVolume
        {
            get
            {
                _audioMixer.SetFloat("EffectVolume", PlayerPrefs.GetFloat("EffectVolume"));
                return PlayerPrefs.GetFloat("EffectVolume");
            }
            set
            {
                PlayerPrefs.SetFloat("EffectVolume", value);

                _audioMixer.SetFloat("EffectVolume", value);
                _effectVolume = value;

                PlayerPrefs.Save();
            }
        }

        private float _musicVolume;
        private float _effectVolume;

        private readonly AudioMixer _audioMixer;

        public PlayerSetting(AudioMixer audioMixer)
        {
            _audioMixer = audioMixer;
            audioMixer.SetFloat("MusicVolume", MusicVolume);
            audioMixer.SetFloat("EffectVolume", EffectVolume);
        }
    }
}
