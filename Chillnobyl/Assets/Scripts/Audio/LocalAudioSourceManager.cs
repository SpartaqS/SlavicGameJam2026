using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class LocalAudioSourceManager : MonoBehaviour
{
    public AudioSource audioSource;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
}
