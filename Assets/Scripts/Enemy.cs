using UnityEngine;
using Zenject;

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour
{
    public class Factory : PlaceholderFactory<Enemy> { }

    [SerializeField]
    float _speed;

    [SerializeField]
    float _accelerations;

    [SerializeField]
    AudioClip _destroySfx;

    Health _health;
    Rigidbody2D _rb;

    float _evaluationTime;
    Vector2 _direction;
    IAudioController _audio;
    Vector3 _lastPosition;

    public void TakeDamage(int dmg)
    {
        _health.TakeDamage(dmg);
    }

    [Inject]
    void Inject(IAudioController audio) => _audio = audio;

    void Start()
    {
        _health = GetComponent<Health>();
        _rb = GetComponent<Rigidbody2D>();
    }

    void OnDestroy()
    {
        if (_audio != null)
            _audio.Play(_destroySfx, Random.Range(0.6f, 0.8f), Random.Range(0.8f, 1));
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // collides with player
        var player = collision.gameObject.GetComponent<PlayerController>();
        if (player != null)
        {
            player.Health.TakeDamage(1);

            var bump = (player.transform.position - transform.position).normalized;
            player.Bump(bump);
        }
    }

    void Update()
    {
        if (_evaluationTime > 0)
        {
            _evaluationTime -= Time.deltaTime;
        }
        else
        {
            _evaluationTime = 0.75f;

            var player = FindObjectOfType<PlayerController>();
            if (player)
            {
                if (Vector3.Distance(transform.position, _lastPosition) > 1)
                {
                    _direction = (player.transform.position - transform.position).normalized;
                }
                else // this is to try to avoid getting stuck on walls
                {
                    var angle = Random.Range(0, 2 * Mathf.PI);
                    _direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
                }
                _lastPosition = transform.position;
            }
            else
            {
                _direction = Vector2.zero;
            }
        }

        _rb.velocity = Vector2.Lerp(_rb.velocity, _direction * _speed, _accelerations * Time.deltaTime);
    }
}
