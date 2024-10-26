using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UIElements;

namespace Game
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioManager : MonoBehaviour
    {
        System.Random random = new System.Random();

        public GameObject SoundMaker;
        private AudioSource _audioPlayer;
        public GameObject[] sounds;

        public const string MasterVolume = "Master";
        public const string MusicVolume = "Music";
        public const string SFXVolume = "SFX";

        [SerializeField] private AudioMixer _audioMixer;

        [SerializeField] private List<AudioClip> _trackList;
        int track_counter = 0;
        private int _currentTrackIndex;

        private void Awake()
        {
            sounds = GameObject.FindGameObjectsWithTag("Sound");
            if (sounds.Length == 0)
            {
                SoundMaker = Instantiate(SoundMaker);
                SoundMaker.name = "SoundMaker";
                DontDestroyOnLoad(SoundMaker.gameObject);
            }
            else
            {
                SoundMaker = GameObject.Find("SoundMaker");
            }

            //_audioPlayer = GetComponent<AudioSource>();
        }

        private void Start()
        {
            _audioPlayer = SoundMaker.GetComponent<AudioSource>();

            Initialize();
            PlayNextTrack();
            track_counter = _trackList.Count;
        }

        private void Initialize()
        {
            //ShuffleTracks();
            _currentTrackIndex = 0;
            _audioPlayer.pitch = 1;
        }

        /// <summary> Проигрывание следующего трека без задержки.</summary>
        private void PlayNextTrack()
        {
            _audioPlayer.clip = _trackList[_currentTrackIndex];
            _audioPlayer.Play();
            StartCoroutine(PlayNextTrackAfterDelay(_audioPlayer.clip.length));
        }

        private IEnumerator PlayNextTrackAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            if (_currentTrackIndex >= _trackList.Count - 1)
            {
                //ShuffleTracks();
                _currentTrackIndex++;
                _currentTrackIndex %= track_counter;
            }
            else
            {
                _currentTrackIndex++;
            }
            PlayNextTrack();
        }

        /*private void ShuffleTracks()
        {
            System.Random random = new();
            for (int n = _trackList.Count - 1; n > 1; n--)
            {
                int k = random.Next(n + 1);
                (_trackList[n], _trackList[k]) = (_trackList[k], _trackList[n]);
            }
        }*/

        public void OnMasterVolumeChanged(System.Single masterLevel)
        {
            SetMixerValue(MasterVolume, masterLevel);
        }

        public void OnMusicVolumeChanged(System.Single musicLevel)
        {
            SetMixerValue(MusicVolume, musicLevel);
        }

        public void OnSFXVolumeChanged(System.Single sfxLevel)
        {
            SetMixerValue(SFXVolume, sfxLevel);
        }

        private void SetMixerValue(string key, System.Single value)
        {
            value = value == 0 ? -80 : Mathf.Log10(value) * 20;
            _audioMixer.SetFloat(key, value);
        }
    }
}