using UniRx;
using UnityEngine;
using Zenject;

public class Health : MonoBehaviour
{
    [SerializeField]
    int _hp;

    [SerializeField]
    int _maxHp;

    public IReactiveProperty<int> Current { get; private set; }
    public IReactiveProperty<float> CurrentPerc { get; private set; }

    IMessageBroker _broker;

    [Inject]
    void Inject(IMessageBroker broker) => _broker = broker;

    void Start()
    {
        Current = new ReactiveProperty<int>(_hp);
        CurrentPerc = new ReactiveProperty<float>(_hp / (float)_maxHp);
        Current.Subscribe(health =>
        {
            CurrentPerc.Value = health / (float)_maxHp;
            _broker.Publish(new HealthChangeEvent(this));
            if (health <= 0)
            {
                _broker.Publish(new EntityDiedEvent(gameObject));
                Destroy(gameObject);
            }
        }).AddTo(this);
    }

    public void TakeDamage(int dmg)
    {
        var c = Current.Value;
        c -= dmg;
        if (c < 0)
            c = 0;
        Current.Value = c;
    }

    public void RestoreHealth(int hp)
    {
        var c = Current.Value;
        c+= hp;
        if (c > _maxHp)
            c = _maxHp;
        Current.Value = c;
    }
}
