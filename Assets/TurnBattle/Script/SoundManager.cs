using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace TurnBaseTest {
    public class SoundManager : MonoBehaviour
    {
        private static SoundManager instance;
        public static SoundManager GetInstance()
        {
            return instance;
        }

        public AudioMixer audioMixer;
        public AudioSource audioSFX;

        public Dictionary<string, AudioClip> sfxData;

        [SerializeField]
        private List<SoundData> sfxClips;

        private void Awake()
        {
            instance = this;
            sfxData = new();

            foreach (var item in sfxClips)
            {
                sfxData.Add(item.name, item.audioClip);
            }
        }

        public void MuteSoundBGM(bool value)
        {
            Debug.Log("[sound] bgm value " + value);
            float volume = (value) ? 0.5f : 0f;
            SetBGMVolume(volume);
            PlayerPrefs.SetInt("bgmSound", (value) ? 1 : 0);
        }

        public void MuteSoundSFX(bool value)
        {
            Debug.Log("[sound] sfx value " + value);
            float volume = (value) ? 0.5f : 0f;
            SetSFXVolume(volume);
            PlayerPrefs.SetInt("sfxSound", (value) ? 1 : 0);
        }

        public void SetSFXVolume(float volume)
        {
            if (volume <= 0)
                volume = 0.0001f;

            audioMixer.SetFloat("Sfx", Mathf.Log10(volume) * 20);
        }

        public void SetBGMVolume(float volume)
        {
            if (volume <= 0)
                volume = 0.0001f;

            Debug.Log("asasasas + " + (Mathf.Log10(volume) * 50));
            audioMixer.SetFloat("BGM", Mathf.Log10(volume) * 50);
        }

        public void PlaySFX(string clipName)
        {
            if (string.IsNullOrEmpty(clipName))
                return;

            audioSFX.PlayOneShot(sfxData[clipName]);
        }
    }

    [System.Serializable]
    public class SoundData
    {
        public string name;
        public AudioClip audioClip;
    }
}

