using Game.Options;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public static class SaveLoadSystem
    {
        private const string DifficultyKey = "Difficulty";
        public const string MasterVolumeKey = "MasterVolume";
        public const string MusicVolumeKey = "MusicVolume";
        public const string SFXVolumeKey = "SFXVolume";

        private static void Start()
        {
            PlayerPrefs.Save();
        }

        public static void SaveSound(SoundSettings settings)
        {
            PlayerPrefs.SetFloat(MasterVolumeKey, settings.Master);
            PlayerPrefs.SetFloat(SFXVolumeKey, settings.SFX);
            PlayerPrefs.SetFloat(MusicVolumeKey, settings.Music);
        }

        public static SoundSettings LoadSound()
        {
            SoundSettings sound = new();
            sound.Master = PlayerPrefs.GetFloat(MasterVolumeKey, 0.5f);
            sound.SFX = PlayerPrefs.GetFloat(SFXVolumeKey, 0.5f);
            sound.Music = PlayerPrefs.GetFloat(MusicVolumeKey, 0.5f);
            return sound;
        }

        public static void SaveDifficulty(int difficultyIndex)
        {
            PlayerPrefs.SetInt(DifficultyKey, difficultyIndex);
            PlayerPrefs.Save();
        }

        public static int LoadDifficulty()
        {
            return PlayerPrefs.GetInt(DifficultyKey, 0);
        }
    }
}