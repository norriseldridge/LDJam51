using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class PlayerProjectileSpawner : MonoBehaviour
{
    [SerializeField]
    InputAction _shoot;

    [SerializeField]
    float _contollerTurnSpeed = 1;

    [SerializeField]
    float _cooldown;

    [SerializeField]
    float _projectileLife;

    [SerializeField]
    float _speed;

    [Inject]
    void Inject(Projectile.Factory factory) => _projectileFactory = factory;

    Projectile.Factory _projectileFactory;
    float _currentCooldown = 0;

    void OnEnable()
    {
        _shoot.Enable();
    }

    void OnDestroy()
    {
        _shoot.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        // aim
        bool usingMouse = Gamepad.current == null;
        if (usingMouse)
        {
            var mousePosition = Mouse.current.position.ReadValue();
            Vector2 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            var direction = (worldPosition - (Vector2)transform.position).normalized;
            transform.up = direction;
        }
        else
        {
            var direction = Gamepad.current.rightStick.ReadValue().normalized;
            if (direction.magnitude > 0)
            {
                transform.up = Vector3.Lerp(transform.up, direction, _contollerTurnSpeed * Time.deltaTime);
            }
        }

        // shoot or cool down
        if (_currentCooldown > 0)
        {
            _currentCooldown -= Time.deltaTime;
        }
        else if (_shoot.IsPressed())
        {
            var proj = _projectileFactory.Create();
            proj.transform.position = transform.position;
            proj.Fire(transform.up * _speed, _projectileLife);

            _currentCooldown = _cooldown;
        }
    }
}
