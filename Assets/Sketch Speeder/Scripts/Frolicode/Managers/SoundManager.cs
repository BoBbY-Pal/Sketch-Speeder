using System;
using Sketch_Speeder.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Sketch_Speeder.Managers
{
    public class SoundManager : Singleton<SoundManager>
    {
        [SerializeField] private Slider musicVolumeSlider;
        [SerializeField] private Slider soundVolumeSlider;
        public AudioSource soundMusic;
        public AudioSource soundEffect;
        public Sounds[] sound;

        void Awake()
        {
            if (PlayerPrefs.HasKey("MUSICVOLUME"))
            {
                float vol = PlayerPrefs.GetFloat("MUSICVOLUME");
                musicVolumeSlider.value = vol;
                // SetMusicVolume(PlayerPrefs.GetFloat("MUSICVOLUME"));
            }
            else
            {
                SetMusicVolume(1);
            }
            
            if (PlayerPrefs.HasKey("SOUNDVOLUME"))
            {
                float vol = PlayerPrefs.GetFloat("SOUNDVOLUME");
                soundVolumeSlider.value = vol;
                // SetSoundVolume();
            }
            else
            {
                SetSoundVolume(1);
            }
            
        }

        public void Play(SoundTypes soundType)
        {
            AudioClip clip = GetSoundClip(soundType);
            if (clip != null)
            {
                soundEffect.PlayOneShot(clip);
            }
            else
            {
                Debug.Log("Clip not found for sound type: " + soundType);
            }
        }

        public void PlayMusic(SoundTypes soundType)
        {

            AudioClip clip = GetSoundClip(soundType);
            if (clip != null)
            {
                soundMusic.clip = clip;
                soundMusic.Play();
            }
            else
            {
                Debug.Log("Clip not found for sound type: " + soundType);
            }
        }

        public void PlayBackgroundMusic()
        {
            PlayMusic(SoundTypes.BackgroundMusic);
        }
        
        public void SetMusicVolume(float val)
        {
            soundMusic.volume = val;
            PlayerPrefs.SetFloat("MUSICVOLUME",val);
        }
        
        public void SetSoundVolume(float val)
        {
            soundEffect.volume = val;
            PlayerPrefs.SetFloat("SOUNDVOLUME",val);
        }

        private AudioClip GetSoundClip(SoundTypes soundType)
        {
            Sounds sounds = Array.Find(sound, item => item.soundType == soundType);
        
            if (sounds != null)
                return sounds.soundClip;
            return null;

        }
    }

    [Serializable]
    public class Sounds
    {
   
        public SoundTypes soundType;
        public AudioClip soundClip;

        public bool b_IsMute = false;
    }
    
    public enum SoundTypes
    {
        None,
        Music,
        BackgroundMusic,
        ButtonClick,
        GameLose,
    }
}