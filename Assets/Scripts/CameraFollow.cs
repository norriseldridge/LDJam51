using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    Transform _target;

    [SerializeField]
    CameraBounds _bounds;

    Vector3 _offset = new Vector3(0, 0, -10);
    Vector2 _cameraSize;

    void Start()
    {
        var verticalSize = Camera.main.orthographicSize * 2.0f;
        var horizontalSize = verticalSize * Screen.width / Screen.height;
        _cameraSize = new Vector2(horizontalSize, verticalSize);
    }

    void Update()
    {
        if (_target)
            transform.position = _bounds.Limit(_target.position + _offset, _cameraSize);
    }
}
