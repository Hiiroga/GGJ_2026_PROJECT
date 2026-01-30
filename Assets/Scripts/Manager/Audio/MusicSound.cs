using UnityEngine;

public class MusicSound: MonoBehaviour
{
    private AudioSource audioSource;

    public AudioClip audioClip;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = audioClip;
        audioSource.loop = true;
        audioSource.Play();
    }
}