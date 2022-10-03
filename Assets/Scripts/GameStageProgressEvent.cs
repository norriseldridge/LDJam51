public class GameStageProgressEvent
{
    public int Stage { get; private set; }

    public GameStageProgressEvent(int stage)
    {
        Stage = stage;
    }
}
