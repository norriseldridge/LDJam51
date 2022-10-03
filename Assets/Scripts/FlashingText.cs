using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class FlashingText : MonoBehaviour
{
    [SerializeField]
    float _speed;

    TextMeshProUGUI _text;

    void Start()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        var show = Mathf.Sin(Time.time * _speed) > -0.5f;
        _text.enabled = show;
    }
}
