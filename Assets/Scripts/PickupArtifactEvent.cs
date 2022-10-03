public class PickupArtifactEvent
{
    public ArtifactData Data { get; private set; }

    public PickupArtifactEvent(ArtifactData data)
    {
        Data = data;
    }
}
