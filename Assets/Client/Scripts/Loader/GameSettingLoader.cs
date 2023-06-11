using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Client
{
    public class GameSettingLoader : ILoadOperation
    {
        public string Description { get; }
        
        private readonly GameSession _gameSession;

        public GameSettingLoader(GameSession gameSession)
        {
            _gameSession = gameSession;
        }

        public Task Load(Action onProgressEnd)
        {
            //_gameSession.PlayerSetting.MasterVolume = InitFloat("MasterVolume");
            _gameSession.PlayerSetting.EffectVolume = InitFloat("EffectVolume");
            _gameSession.PlayerSetting.MusicVolume = InitFloat("MusicVolume");
            
            onProgressEnd.Invoke();

            return Task.CompletedTask;
        }

        private float InitFloat(string key)
        {
            if (PlayerPrefs.HasKey(key))
                return PlayerPrefs.GetFloat(key);
            PlayerPrefs.SetFloat(key, 0f);
            return PlayerPrefs.GetFloat(key);
        }
    }
}