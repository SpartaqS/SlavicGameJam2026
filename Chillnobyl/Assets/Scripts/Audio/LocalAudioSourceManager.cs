using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class LocalAudioSourceManager : MonoBehaviour
{
    public AudioSource audioSource;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayInLoop(SoundManager.Sound sound)
    {
        if (audioSource.isPlaying)
            return;
        audioSource.loop = true;
        SoundManager._Instance.PlaySound(audioSource, sound);
    }

    public void PlayOneShot(SoundManager.Sound sound)
    {
        audioSource.loop = false;
        SoundManager._Instance.PlaySound(audioSource, sound);
    }

    public void StopSound()
    {
        SoundManager._Instance.StopSound(audioSource);
    }

    public void SetPitch(float pitch)
    {
        audioSource.pitch = pitch;
    }

    public void SetVolume(float volume)
    {
        audioSource.volume = Mathf.Clamp01(volume);
    }
}
