using UnityEngine;
using UnityEngine.UI;

namespace Game.Options
{
    public struct SoundSettings
    {
        public float Master;
        public float SFX;
        public float Music;
    }

    public class AudioOptions : MonoBehaviour
    {
        [SerializeField] private Slider _masterVolume;
        [SerializeField] private Slider _sfxVolume;
        [SerializeField] private Slider _musicVolume;

        private SoundSettings _currentSettings;

        private void Start()
        {
            LoadSettings();
        }

        private void Update()
        {
            if (Input.GetMouseButtonUp(0))
            {
                SaveSoundSettings();
            }
        }

        private void LoadSettings()
        {
            _currentSettings = SaveLoadSystem.LoadSound();
            _masterVolume.value = _currentSettings.Master;
            _sfxVolume.value = _currentSettings.SFX;
            _musicVolume.value = _currentSettings.Music;
        }

        public void SetMasterVolume(float volume)
        {
            _currentSettings.Master = volume;
        }

        public void SetSFXVolume(float volume)
        {
            _currentSettings.SFX = volume;
        }

        public void SetMusicVolume(float volume)
        {
            _currentSettings.Music = volume;
        }

        private void SaveSoundSettings()
        {
            SaveLoadSystem.SaveSound(_currentSettings);
        }
    }
}
