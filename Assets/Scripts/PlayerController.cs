using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Health))]
public class PlayerController : MonoBehaviour
{
    public Health Health { get; private set; }

    [SerializeField]
    InputAction _movement;

    [SerializeField]
    InputAction _dash;

    [SerializeField]
    float _speed = 1.0f;

    [SerializeField]
    float _dashSpeed = 1.0f;

    [SerializeField]
    float _acceleration = 1.0f;

    [SerializeField]
    AudioClip _hitSfx;

    Rigidbody2D _rb;

    IMessageBroker _broker;
    IAudioController _audio;
    float _dashCooldownTime = 0;
    float _dashTime = 0;
    int _previousHealth = int.MinValue;

    public void Bump(Vector2 bump)
    {
        _rb.AddForce(bump * _speed, ForceMode2D.Impulse);
    }

    [Inject]
    void Inject(IMessageBroker broker, IAudioController audio)
    {
        _broker = broker;
        _audio = audio;
    }

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        Health = GetComponent<Health>();

        _broker.Receive<HealthChangeEvent>()
            .Where(e => e.Health == Health)
            .Subscribe(e =>
            {
                var health = e.Health.Current.Value;
                if (health < _previousHealth)
                {
                    _audio.Play(_hitSfx,
                        Random.Range(0.3f, 0.4f),
                        Random.Range(0.4f, 0.6f));
                }
                _previousHealth = health;
            })
            .AddTo(this);
    }

    void OnEnable()
    {
        _movement.Enable();
        _dash.Enable();
    }

    void OnDestroy()
    {
        _movement.Disable();
        _dash.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        if (_dashTime > 0)
            _dashTime -= Time.deltaTime;

        if (_dashCooldownTime > 0)
            _dashCooldownTime -= Time.deltaTime;

        if (CanDash && _dash.WasPressedThisFrame())
        {
            _dashTime = 0.25f;
            _dashCooldownTime = 0.65f;
        }

        var movement = _movement.ReadValue<Vector2>().normalized;
        _rb.velocity = Vector2.Lerp(_rb.velocity, movement * Speed, Acceleration * Time.deltaTime);
    }

    bool CanDash => _dashCooldownTime <= 0;
    bool IsDashing => _dashTime > 0;
    float Speed => IsDashing ? _dashSpeed : _speed;
    float Acceleration => IsDashing ? _dashSpeed : _acceleration;
}
