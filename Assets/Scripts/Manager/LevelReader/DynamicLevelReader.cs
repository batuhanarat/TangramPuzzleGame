
public class DynamicLevelReader : BaseLevelReader
{

    #region Private Variables

        private LevelConfig levelConfig;

    #endregion

    public DynamicLevelReader() : base()
    {
    }

    public override Level LoadLevel()
    {
        levelConfig = ServiceProvider.AssetLibrary.GetLevelConfig();
        LevelData = new()
        {
            BoardSize = levelConfig.boardSize,
            Seed = levelConfig.seed,
            PieceCount = levelConfig.pieceCount
        };

        return LevelData;
    }


    public override void PrepareNextLevel()
    {
        levelConfig.NextLevel();
        LevelData.Seed = levelConfig.seed;
    }


}