using System;
using UnityEngine;
using UnityEngine.Audio;
using Object = System.Object;

namespace Client
{
    public class GameSession : MonoBehaviour
    {
        [SerializeField] private AudioMixer _audioMixer;

        public AudioMixer AudioMixer => _audioMixer;
    
        private int _money;

        public int Money
        {
            get
            {
                if (PlayerPrefs.HasKey("Money"))
                    return PlayerPrefs.GetInt("Money");

                PlayerPrefs.SetInt("Money", 500);
                PlayerPrefs.Save();

                return PlayerPrefs.GetInt("Money");
            }
            set
            {
                _money = value;
                PlayerPrefs.SetInt("Money", _money);
                MoneyChanged?.Invoke(_money);
                PlayerPrefs.Save();
            }
        }

        public event Action <int> MoneyChanged;
    
        public PlayerSetting PlayerSetting { get; private set; }

        private void Awake()
        {
            PlayerSetting = new PlayerSetting(_audioMixer);
        }


        private void Start() => CheckGameSessionComponents(_audioMixer);

        private void CheckGameSessionComponents(params Object[] components)
        {
            foreach (var component in components)
            {
                if (ReferenceEquals(component, null)) Debug.LogError($"Some Components has a problem in Game Session");
            }
        }
    }
}
