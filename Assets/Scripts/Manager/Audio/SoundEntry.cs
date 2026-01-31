using UnityEngine;

[System.Serializable]
public class SoundEntry
{
    public string key;
    public AudioClip clip;
    [Range(0f, 1f)] public float volume = 1f;
}