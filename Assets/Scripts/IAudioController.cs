using UnityEngine;

public interface IAudioController
{
    void PlayGameMusic();
    void Play(AudioClip clip, float volume, float pitch);
}
