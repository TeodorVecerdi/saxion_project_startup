using System;
using System.Collections.Generic;
using UnityCommons;
using UnityEngine;

public class SoundManager : MonoSingleton<SoundManager> {
    public List<AudioKeyValuePair> SoundList;
    private Dictionary<string, AudioSource> Sounds {
        get {
            if (soundDictionary != null) {
                return soundDictionary;
            }

            soundDictionary = new Dictionary<string, AudioSource>();
            foreach (var sound in SoundList) {
                var audioSource = gameObject.AddComponent<AudioSource>();
                audioSource.clip = sound.Sound;
                audioSource.loop = false;
                audioSource.playOnAwake = false;
                soundDictionary[sound.Key] = audioSource;
            }

            return soundDictionary;
        }
    }

    private Dictionary<string, AudioSource> soundDictionary;
    

    public static void PlaySound(string soundKey, bool stopIfPlaying = false, bool skipIfAlreadyPlaying = false) {
        if (Instance.Sounds[soundKey] != null) {
            if (stopIfPlaying)
                Instance.Sounds[soundKey].Stop();
            
            if(skipIfAlreadyPlaying && Instance.Sounds[soundKey].isPlaying) 
                return;
            
            var offset = 0f;
            
            
            if (Instance.Sounds[soundKey].loop)
                Instance.Sounds[soundKey].Play();
            else
                Instance.Sounds[soundKey].PlayOneShot(Instance.Sounds[soundKey].clip);

            Instance.Sounds[soundKey].time = offset;
        }
    }

    public static void StopSound(string soundKey) {
        if (Instance.Sounds[soundKey] != null) {
            Instance.Sounds[soundKey].Stop();
        }
    }

    public static bool IsPlaying(string soundKey) {
        return Instance.Sounds[soundKey] != null && Instance.Sounds[soundKey].isPlaying;
    }

    [Serializable]
    public class AudioKeyValuePair {
        [SerializeField] public string Key;
        [SerializeField] public AudioClip Sound;
    }
}