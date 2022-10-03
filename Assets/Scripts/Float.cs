using UnityEngine;

public class Float : MonoBehaviour
{
    [SerializeField]
    float _speed;

    [SerializeField]
    float _distance;

    // Update is called once per frame
    void Update()
    {
        var angle = Time.time * _speed;
        transform.localPosition = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)) * _distance;
    }
}
