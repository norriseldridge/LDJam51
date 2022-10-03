using UnityEngine;
using UniRx;
using Zenject;

public class TimeController : MonoBehaviour, IGameTimer
{
    // the current time in seconds
    public IReactiveProperty<float> CurrentTime => _time;
    IReactiveProperty<float> _time = new ReactiveProperty<float>(10.0f);

    // the current time as a percent
    public IReactiveProperty<float> CurrentTimeProgress => _progress;
    IReactiveProperty<float> _progress = new ReactiveProperty<float>(1.0f);

    IMessageBroker _broker;

    [Inject]
    void Inject(IMessageBroker broker) => _broker = broker;

    void Update()
    {
        _time.Value -= Time.deltaTime;
        if (_time.Value < 0.0f)
        {
            _time.Value = 10.0f;
            _broker.Publish(new TenSecondsPassedEvent());
        }

        _progress.Value = _time.Value / 10.0f;
    }
}
