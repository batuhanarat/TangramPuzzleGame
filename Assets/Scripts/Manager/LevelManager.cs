using UnityEngine;
using UnityEngine.SceneManagement;

public interface ILevelManager
{
    public Difficulty LevelDifficulty { get; }
    public int Seed { get; }
    public int PieceCount { get; }
    public int BoardSize { get; }

    public void SetupLevel();
    public void PrepareNextLevel();
}

public class LevelManager : IProvidable , ILevelManager
{

    #region Private Variables

        private Level _level;
        private ILevelReader _levelReader;

    #endregion

    #region Properties

        public Difficulty LevelDifficulty { get; private set; }
        public int PieceCount { get; private set; }
        public int Seed { get; private set; }
        public int BoardSize { get; private set; }

    #endregion

    public LevelManager()
    {
        ServiceProvider.Register(this);
        _levelReader = ServiceProvider.LevelReader;
    }

    public void SetupLevel()
    {
        _level = _levelReader.LoadLevel();


        if (!ValidateLevel(_level))
        {
            Debug.LogError("Level validation failed.");
            return;
        }

        PieceCount = _level.PieceCount;
        Seed = _level.Seed;
        BoardSize = _level.BoardSize ;

        var difficulty = DetermineDifficulty(_level.PieceCount);

        if (!ValidateDifficulty(difficulty, _level.BoardSize))
        {
            Debug.LogError("Difficulty level is not expected");
            return;
        }

        LevelDifficulty = difficulty;
        Debug.Log("Level setup completed with difficulty: " + LevelDifficulty);

    }

    private bool ValidateLevel(Level level)
    {
        if(level == null)
        {
            Debug.LogError("Level is null");
            return false;
        }
        if (level.PieceCount < 5 || level.PieceCount > 12)
        {
            Debug.LogError("Level piece count is wrong");
            return false;
        }
        if(level.BoardSize < 4 || level.BoardSize > 6)
        {
            Debug.LogError("Level board size is wrong");
            return false;
        }

        return true;
    }

    private Difficulty DetermineDifficulty(int pieceCount)
    {
        return pieceCount switch
        {
            > 10 => Difficulty.HARD,
            > 7 => Difficulty.MEDIUM,
            > 3 => Difficulty.EASY,
            _ => Difficulty.INVALID
        };
    }

    private bool ValidateDifficulty(Difficulty difficulty, int boardSize)
    {
        Difficulty expectedDifficulty = boardSize switch
        {
            4 => Difficulty.EASY,
            5 => Difficulty.MEDIUM,
            6 => Difficulty.HARD,
            _ => Difficulty.INVALID,
        };

        return difficulty == expectedDifficulty;
    }

    public void PrepareNextLevel()
    {
        _levelReader.PrepareNextLevel();
        ServiceProvider.BoardRenderer.PlayWinAnimation(
            LoadNextLevel
            );
    }

    private void LoadNextLevel()
    {
        SceneManager.LoadScene(Constants.SceneName);
    }

}