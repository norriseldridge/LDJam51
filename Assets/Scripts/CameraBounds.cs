using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class CameraBounds : MonoBehaviour
{
    BoxCollider2D _collider;

    // Start is called before the first frame update
    void Start()
    {
        _collider = GetComponent<BoxCollider2D>();
    }

    public Vector3 Limit(Vector3 position, Vector2 cameraSize)
    {
        var minX = _collider.bounds.min.x + cameraSize.x / 2;
        var maxX = _collider.bounds.max.x - cameraSize.x / 2;
        var minY = _collider.bounds.min.y + cameraSize.y / 2;
        var maxY = _collider.bounds.max.y - cameraSize.y / 2;

        if (minX > maxX)
            minX = maxX = _collider.bounds.center.x;

        if (minY > maxY)
            minY = maxY = _collider.bounds.center.y;

        return new Vector3(
            Mathf.Clamp(position.x, minX, maxX),
            Mathf.Clamp(position.y, minY, maxY),
            position.z);
    }
}
