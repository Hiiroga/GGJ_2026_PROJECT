public class MusicManager : AudioManagerBase<MusicManager>
{
    protected override void Awake()
    {
        base.Awake();
        if (audioSource != null)
            audioSource.loop = true;
    }

    public override void Play(string key)
    {
        var sound = GetSound(key);
        if (sound == null) return;

        audioSource.clip = sound.clip;
        audioSource.volume = sound.volume;
        audioSource.Play();
    }

    public override void Stop()
    {
        audioSource.Stop();
    }

    public void Pause() => audioSource.Pause();
    public void UnPause() => audioSource.UnPause();
}