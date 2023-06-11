using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Client
{
    public class SettingWindow : BaseWindow
    {
        [SerializeField] private Slider _musicSlider;
        [SerializeField] private Slider _effectSlider;

        [SerializeField] private Button[] _buttons;

        private GameSession _gameSession;

        [Inject]
        public void Constructor(GameSession gameSession) => _gameSession = gameSession;

        private void Start()
        {
            _effectSlider.value = 100f;
            _musicSlider.value = 100f;
        }


        protected override void OnEnable()
        {
            base.OnEnable();
            _musicSlider.onValueChanged.AddListener(OnMusicSliderChanged);
            _effectSlider.onValueChanged.AddListener(OnEffectSliderChanged);


            _buttons[0].onClick.AddListener(OnSoundButtonPlay);
            _buttons[1].onClick.AddListener(OnSoundButtonStop);

            _buttons[2].onClick.AddListener(OnMusicButtonPlay);
            _buttons[3].onClick.AddListener(OnMusicButtonStop);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            //_languageButton.onClick.RemoveListener(OnLanguageButton);

            _musicSlider.onValueChanged.RemoveListener(OnMusicSliderChanged);
            _effectSlider.onValueChanged.RemoveListener(OnEffectSliderChanged);


            _buttons[0].onClick.RemoveListener(OnSoundButtonPlay);
            _buttons[1].onClick.RemoveListener(OnSoundButtonStop);

            _buttons[2].onClick.RemoveListener(OnMusicButtonPlay);
            _buttons[3].onClick.RemoveListener(OnMusicButtonStop);
        }


        private void OnMusicSliderChanged(float value)
        {
            var result = value.Remap(_musicSlider.minValue, _musicSlider.maxValue, -50f, 10f);
            _gameSession.PlayerSetting.MusicVolume = result;
        }

        private void OnEffectSliderChanged(float value)
        {
            var result = value.Remap(_effectSlider.minValue, _musicSlider.maxValue, -50f, 10f);
            _gameSession.PlayerSetting.EffectVolume = result;
        }


        private void OnSoundButtonPlay()
        {
            _effectSlider.value = 100f;
        }

        private void OnMusicButtonPlay()
        {
            _musicSlider.value = 100f;
        }

        private void OnSoundButtonStop()
        {
            _effectSlider.value = 0f;
        }

        private void OnMusicButtonStop()
        {
            _musicSlider.value = 0f;
        }
    }
}