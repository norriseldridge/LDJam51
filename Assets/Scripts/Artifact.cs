using UniRx;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(CircleCollider2D))]
public class Artifact : MonoBehaviour
{
    public class Factory : PlaceholderFactory<Artifact> { }

    [SerializeField]
    SpriteRenderer _renderer;

    IMessageBroker _broker;
    ArtifactData _data;

    [Inject]
    void Inject(IMessageBroker broker)
    {
        _broker = broker;
    }

    public void SetData(ArtifactData data)
    {
        _data = data;
        _renderer.sprite = _data.Sprite;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>())
        {
            _broker.Publish(new PickupArtifactEvent(_data));
            Destroy(gameObject);
        }
    }
}
