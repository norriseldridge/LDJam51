using UniRx;

public interface IGameTimer
{
    IReactiveProperty<float> CurrentTime { get; }
    IReactiveProperty<float> CurrentTimeProgress { get; }
}
