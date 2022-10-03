using UnityEngine;
using Zenject;

public class Projectile : MonoBehaviour
{
    public class Factory : PlaceholderFactory<Projectile> { }

    [SerializeField]
    Particle _source;

    [SerializeField]
    AudioClip _shootSfx;

    [SerializeField]
    AudioClip _blowUpSfx;

    Vector3 _direction;
    float _life;
    IAudioController _audio;

    [Inject]
    void Inject(IAudioController audioController) => _audio = audioController;

    public void Fire(Vector2 direction, float lifeTime)
    {
        _direction = direction;
        _life = lifeTime;

        _audio.Play(_shootSfx, Random.Range(0.5f, 0.7f), Random.Range(0.8f, 1));
    }

    public void Blowup()
    {
        _audio.Play(_blowUpSfx, Random.Range(0.8f, 1), Random.Range(0.8f, 1));

        for (int i = 0; i < 10; i++)
        {
            var part = Instantiate(_source);
            part.transform.position = transform.position;
            var angle = Random.Range(0, 2 * Mathf.PI);
            var force = Random.Range(10, 15);
            part.Fire(new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * force, 1);
        }

        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (_life > 0)
        {
            _life -= Time.deltaTime;
            transform.position += _direction * Time.deltaTime;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
