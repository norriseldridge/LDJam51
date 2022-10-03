public class HealthChangeEvent
{
    public Health Health { get; private set; }

    public HealthChangeEvent(Health health) => Health = health;
}
