using System.Collections.Generic;
using UnityEngine;

public abstract class AudioManagerBase<T> : MonoBehaviour where T : AudioManagerBase<T>
{
    public static T Instance { get; private set; }

    [SerializeField] protected SoundEntry[] sounds;
    protected AudioSource audioSource;
    protected Dictionary<string, SoundEntry> soundDict;

    protected virtual void Awake()
    {
        if (Instance == null)
        {
            Instance = (T)this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        audioSource = GetComponent<AudioSource>();
        BuildDictionary();
    }

    private void BuildDictionary()
    {
        soundDict = new Dictionary<string, SoundEntry>();
        foreach (var entry in sounds)
        {
            if (!string.IsNullOrEmpty(entry.key))
                soundDict[entry.key] = entry;
        }
    }

    protected SoundEntry GetSound(string key)
    {
        if (soundDict.TryGetValue(key, out var entry))
            return entry;

        Debug.LogWarning($"[{typeof(T).Name}] Sound '{key}' not found!");
        return null;
    }

    public abstract void Play(string key);
    public abstract void Stop();
    public virtual void SetVolume(float volume) => audioSource.volume = volume;
}