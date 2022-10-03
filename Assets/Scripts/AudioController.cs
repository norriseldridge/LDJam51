using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour, IAudioController
{
    [SerializeField]
    AudioClip _gameMusic;

    [SerializeField]
    AudioSource _music;

    int _max = 20;
    Queue<AudioSource> _sources = new Queue<AudioSource>();

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void PlayGameMusic()
    {
        _music.loop = false;
        StartCoroutine(WaitForEndThenStart());
    }

    IEnumerator WaitForEndThenStart()
    {
        while (_music.isPlaying)
            yield return null;
        _music.clip = _gameMusic;
        _music.loop = true;
        _music.Play();
    }

    public void Play(AudioClip clip, float volume, float pitch)
    {
        var source = Next();
        source.pitch = pitch;
        source.volume = volume;
        source.PlayOneShot(clip);
    }

    AudioSource Next()
    {
        if (_sources.Count < _max)
        {
            _sources.Enqueue(gameObject.AddComponent<AudioSource>());
        }

        var next = _sources.Dequeue();
        _sources.Enqueue(next);
        return next;
    }
}
