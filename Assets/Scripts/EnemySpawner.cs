using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Zenject;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    Light2D _light;

    [SerializeField]
    AudioClip _spawnSfx;

    IAudioController _audio;
    Enemy.Factory _factory;

    [Inject]
    void Inject(IAudioController audio, Enemy.Factory factory)
    {
        _audio = audio;
        _factory = factory;
    }

    void Start()
    {
        _light.intensity = 0;
    }

    public void Spawn()
    {
        StartCoroutine(FlickerLights());
        var enemy = _factory.Create();
        enemy.transform.position = transform.position
            + new Vector3(Random.Range(-0.25f, 0.25f), Random.Range(-0.25f, 0.25f));
    }

    IEnumerator FlickerLights()
    {
        var listener = FindObjectOfType<AudioListener>();
        var dist = Vector2.Distance(transform.position, listener.transform.position);
        var maxDistance = 15;
        var vol = dist / maxDistance;
        _audio.Play(_spawnSfx, Random.Range(0.8f, 0.9f) * vol, Random.Range(1, 1.1f));
        for (int i = 0; i < 11; ++i)
        {
            _light.intensity = (i % 2 == 0) ? Random.Range(1, 2) : 0;
            yield return new WaitForSeconds(Random.Range(0.05f, 0.2f));
        }
        _light.intensity = 0;
    }
}
