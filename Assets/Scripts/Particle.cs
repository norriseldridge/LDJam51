using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Rigidbody2D))]
public class Particle : MonoBehaviour
{
    Light2D _light;
    float _intensity;
    float _life;
    float _maxLife;

    public void Fire(Vector2 direction, float life)
    {
        _light = GetComponentInChildren<Light2D>();
        _intensity = _light.intensity;
        _life = life;
        _maxLife = _life;
        var rb = GetComponent<Rigidbody2D>();
        rb.AddForce(direction, ForceMode2D.Impulse);
    }

    void Update()
    {
        _life -= Time.deltaTime;
        if (_life <= 0)
        {
            Destroy(gameObject);
        }
        else
        {
            float perc = _life / _maxLife;
            transform.localScale = Vector3.one * perc;
            _light.intensity = _intensity * perc;
        }
    }
}
