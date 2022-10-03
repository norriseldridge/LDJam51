using UnityEngine;

public class HeartPulse : MonoBehaviour
{
    [SerializeField]
    float _scaleMin;

    [SerializeField]
    float _scaleMax;

    [SerializeField]
    float _scaleSpeed;

    float _scaleRange;

    void Start()
    {
        _scaleRange = (_scaleMax - _scaleMin) * 0.5f;
    }

    void Update()
    {
        var scale = _scaleMin + (1 + Mathf.Sin(Time.time * _scaleSpeed)) * _scaleRange;
        transform.localScale = Vector3.one * scale;
    }
}
