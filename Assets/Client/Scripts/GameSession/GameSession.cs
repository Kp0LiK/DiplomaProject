using System;
using UnityEngine;
using UnityEngine.Audio;
using Object = System.Object;

namespace Client
{
    public enum GameStateType
    {
        InMenu,
        InGame,
        InPause,
        InLoose
    }
    public class GameSession : MonoBehaviour
    {
        [SerializeField] private AudioMixer _audioMixer;

        public AudioMixer AudioMixer => _audioMixer;
        
        public GameStateType GameState { get; private set; }

        private int _money;

        public int Money
        {
            get
            {
                if (PlayerPrefs.HasKey("Money"))
                    return PlayerPrefs.GetInt("Money");

                PlayerPrefs.SetInt("Money", 0);
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
        public event Action<GameStateType> GameStateChanged;

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
        
        public void ChangeGameState(GameStateType gameStateType)
        {
            GameState = gameStateType;
            GameStateChanged?.Invoke(gameStateType);
        }
    }
}
