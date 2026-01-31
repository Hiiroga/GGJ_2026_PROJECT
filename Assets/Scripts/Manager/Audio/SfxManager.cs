using UnityEngine;

public class SfxManager : AudioManagerBase<SfxManager>
{
    public override void Play(string key)
    {
        var sound = GetSound(key);
        if (sound == null) return;

        audioSource.PlayOneShot(sound.clip, sound.volume);
    }

    public override void Stop()
    {
        audioSource.Stop();
    }

    public void PlayOneShot(AudioClip clip, float volume = 1f)
    {
        audioSource.PlayOneShot(clip, volume);
    }
}