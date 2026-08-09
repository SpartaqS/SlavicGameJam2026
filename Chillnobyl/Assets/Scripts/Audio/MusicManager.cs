using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public enum Sound
    {
        game_music
    }

    [SerializeField]
    AudioSource musicAudioSource = null;

    public static MusicManager instance = null;

    [SerializeField]
    [Tooltip("Set music tracks to play on this level")]
    Sound[] musicPlaylist = new Sound[1];

    [SerializeField]
    [Tooltip("Do not edit")]
    AudioClipOrganized[] audioClipArray = new AudioClipOrganized[1];

    private void Awake()
    {
        instance = this;
        StartCoroutine(playMusic(0));
    }

    IEnumerator playMusic(int trackIndex)
    {
        int newTrackIndex = trackIndex;
        while (true)
        {
            musicAudioSource.PlayOneShot(GetSoundClip(musicPlaylist[newTrackIndex]));
            yield return new WaitForSeconds(GetSoundClip(musicPlaylist[newTrackIndex]).length);
            ++newTrackIndex;
            if (newTrackIndex >= musicPlaylist.Length)
            {
                newTrackIndex = 0;
            }
        }
    }

    public void StopMusic()
    {
        StopAllCoroutines();
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

    [System.Serializable]
    public class AudioClipOrganized
    {
        public Sound audio;
        public AudioClip audioClip;
    }
}