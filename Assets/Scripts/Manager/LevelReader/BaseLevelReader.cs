public abstract class BaseLevelReader : IProvidable, ILevelReader
{
    public Level LevelData {get; protected set;}

    protected BaseLevelReader()
    {
        ServiceProvider.Register(this);
    }

    public abstract void PrepareNextLevel();
    public abstract Level LoadLevel();

}