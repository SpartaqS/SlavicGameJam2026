using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public enum Sound
    {
        Pump,
        ButtonDown,
        ButtonUp,
    }

    public static SoundManager _Instance = null;

    [SerializeField]
    AudioClipOrganized[] audioClipArray = new AudioClipOrganized[1];

    private Dictionary<Sound, float> soundTimerDictionary = new Dictionary<Sound, float>();

    private void Awake()
    {
        _Instance = this;
    }

    public void PlaySound(AudioSource audioSource, Sound sound)
    {
        if (CanPlaySound(sound))
        {
            audioSource.PlayOneShot(GetSoundClip(sound));
        }
    }

    public void StopSound(AudioSource audioSource)
    {
        audioSource.Stop();
    }

    private AudioClip GetSoundClip(Sound audio)
    {
        foreach (AudioClipOrganized audioClipOrganized in audioClipArray)
        {
            if (audioClipOrganized.audio == audio)
            {
                return audioClipOrganized.audioClip;
            }
        }
        return null;
    }

    public void ResetSoundTimer(Sound sound)
    {
        if (soundTimerDictionary.ContainsKey(sound))
        {
            soundTimerDictionary.Remove(sound);
        }
    }

    private bool CanPlaySound(Sound sound)
    {
        switch (sound)
        {
            default:
                return true;
            case Sound.Pump:
                if (soundTimerDictionary.ContainsKey(sound))
                {
                    float lastTimePlayed = soundTimerDictionary[sound];
                    float pumpSFXlength = GetSoundClip(Sound.Pump).length;
                    if (lastTimePlayed + pumpSFXlength < Time.time)
                    {
                        soundTimerDictionary[sound] = Time.time;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    soundTimerDictionary.Add(sound, Time.time);
                    return true;
                }
        }
    }

    [System.Serializable]
    public class AudioClipOrganized
    {
        public Sound audio;
        public AudioClip audioClip;
    }
}
