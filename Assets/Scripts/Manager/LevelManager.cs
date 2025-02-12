using UnityEngine.SceneManagement;

public interface ILevelManager
{
    public void PrepareNextLevel();
}

public class LevelManager : IProvidable , ILevelManager
{

    private LevelSo level;

    public LevelManager()
    {
        ServiceProvider.Register(this);
        level = ServiceProvider.Level;
    }
    public void PrepareNextLevel()
    {
        level.NextLevel();
        ServiceProvider.BoardRenderer.PlayWinAnimation(
            LoadNextLevel
            );
    }

    private void LoadNextLevel()
    {
        SceneManager.LoadScene(0);
    }
}